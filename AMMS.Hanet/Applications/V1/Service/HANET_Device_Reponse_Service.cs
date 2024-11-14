using AMMS.DeviceData.RabbitMq;
using AMMS.Hanet.Data.Response;
using AMMS.Hanet.Datas.Databases;
using AMMS.Hanet.Datas.Entities;
using EventBus.Messages;
using Newtonsoft.Json;
using Shared.Core.Loggers;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Images;
using NetTopologySuite.Index.HPRtree;

namespace AMMS.Hanet.Applications.V1.Service
{
    /// <summary>
    /// Xử lý data
    /// </summary>
    public class HANET_Device_Reponse_Service
    {
        private readonly IEventBusAdapter _eventBusAdapter;
        private readonly IConfiguration _configuration;

        DeviceAutoPushDbContext _deviceAutoPushDbContext;
        ViettelDbContext _viettelDbContext;
        public HANET_Device_Reponse_Service(DeviceAutoPushDbContext deviceAutoPushDbContext, IEventBusAdapter eventBusAdapter, IConfiguration configuration1, ViettelDbContext viettelDbContext)
        {
            _deviceAutoPushDbContext = deviceAutoPushDbContext;
            _eventBusAdapter = eventBusAdapter;
            _configuration = configuration1;
            _viettelDbContext = viettelDbContext;
        }
        /// <summary>
        /// Xử lý thông tin thiết bị trả về
        /// </summary>
        /// <returns></returns>
        public async Task ProcessData(Hanet_Device_Data reponse)
        {
            if (reponse == null || string.IsNullOrEmpty(reponse.data))
            {
                return;
            }

            string datatype = JObject.Parse(reponse.data)["data_type"].ToString();
            //Chỉ sử lý dạng log
            if (datatype == "log")
            {
                Logger.Warning(reponse.data);
                var data = JsonConvert.DeserializeObject<Hanet_Checkin_Data>(reponse.data);

                if (data == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(data.aliasID))
                    return;

                //Tìm học sinh
                var user = await FindUserFromHanet(data.aliasID);
                //Lưu thông tin 
                await AddTransactionLog(data, reponse);

                if (user == null) return;

                //Đẩy sang SV
                await AddATTLOG(data, user);

            }

        }

        private async Task AddATTLOG(Hanet_Checkin_Data data, Student? student)
        {
            try
            {
                #region Gửi lên RBMQ
                try
                {
                    TA_AttendenceHistory tA_AttendenceHistory = new TA_AttendenceHistory()
                    {
                        Id = data.id,
                        DeviceId = data.deviceID,
                        PersonCode = student.StudentCode,
                        SerialNumber = data.deviceName,
                        TimeEvent = data.date,
                        Type = (int)TA_AttendenceType.Face,
                    };

                    RB_DataResponse rB_Response = new RB_DataResponse()
                    {
                        Id = tA_AttendenceHistory.Id,
                        Content = JsonConvert.SerializeObject(tA_AttendenceHistory),
                        ReponseType = RB_DataResponseType.AttendenceHistory,
                    };

                    var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Data_Auto_Push_D2S}");

                    await aa.Send(rB_Response);


                    if (string.IsNullOrEmpty(data.detected_image_url))
                    {
                        return;
                    }

                    await AddATTImage(data, student);
                }
                catch (Exception e)
                {
                    Logger.Warning(e.Message);

                }


                #endregion
            }
            catch (Exception ex)
            {

                Logger.Warning(ex.Message);
            }
        }

        private async Task AddATTImage(Hanet_Checkin_Data data, Student student)
        {
            try
            {
                var image = ConvertImage(data.detected_image_url, data.id + ".jpg", ImageFormat.Jpeg);


                TA_AttendenceImage tA_AttendenceImage = new TA_AttendenceImage()
                {
                    Id = Guid.NewGuid().ToString(),
                    ImageBase64 = image,
                    PersonCode = student.StudentCode,
                    SerialNumber = data.deviceID,
                    TimeEvent = data.date,
                    AttendenceHistoryId = data.id,
                };

                RB_DataResponse rB_DataResponse = new RB_DataResponse()
                {
                    Id = tA_AttendenceImage.Id,
                    Content = JsonConvert.SerializeObject(tA_AttendenceImage),
                    ReponseType = RB_DataResponseType.AttendenceImage,
                };

                var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Data_Auto_Push_D2S}");
                await aa.Send(rB_DataResponse);
            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
            }
        }
        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task AddUserData(Hanet_User_Data data)
        {
            try
            {
                string imageUrl = data.avatar;

                if (imageUrl == null) return;
                Uri uri = new Uri(data.avatar);
                string fileName = Path.GetFileName(uri.LocalPath);

                var image = ConvertImage(imageUrl, fileName, ImageFormat.Jpeg);

                var id = fileName.Split('.')[0];

                TA_PersonInfo obj = new TA_PersonInfo()
                {
                    Id = id,
                    PersonCode = data.aliasID,
                    SerialNumber = "",
                    DeviceModel = EventBusConstants.HANET,
                    FullName = data.name,
                    UserFace = image,
                };

                RB_DataResponse rB_DataResponse = new RB_DataResponse()
                {
                    Id = obj.Id,
                    Content = JsonConvert.SerializeObject(obj),
                    ReponseType = RB_DataResponseType.UserInfo,
                };

                var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Data_Auto_Push_D2S}");
                await aa.Send(rB_DataResponse);
            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
            }
        }
        /// <summary>
        /// Tìm học sinh trong CSDL
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<Student> FindUserFromHanet(string code)
        {
            try
            {
                var user = await _viettelDbContext.Student.FirstOrDefaultAsync(x => x.SyncCode == code || x.StudentCode == code);
                return user;
            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
                return null;
            }
        }

        public string? ConvertImage(string imageUrl, string filename, ImageFormat format)
        {

            string? strImage = null;
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imageUrl);
            Bitmap bitmap; bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                using (var ms = new MemoryStream())
                {

                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    strImage = Convert.ToBase64String(ms.GetBuffer());

                }
            }

            stream.Flush();
            stream.Close();
            client.Dispose();

            return strImage;
        }
        private async Task AddTransactionLog(Hanet_Checkin_Data data, Hanet_Device_Data reponse)
        {
            try
            {
                hanet_transaction hanet_Transaction = new hanet_transaction()
                {
                    id = data.id,
                    content = reponse.data,
                    deviceID = data.deviceID,
                    time = data.time,
                    transaction_type = TransactionRealTime,
                    created_time = DateTime.Now,
                };
                _deviceAutoPushDbContext.Add(hanet_Transaction);
                _deviceAutoPushDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
        /// <summary>
        /// Realtime
        /// </summary>
        public const string TransactionRealTime = "Realtime";

        /// <summary>
        /// History
        /// </summary>
        public const string TransactionHistory = "History";


        public async Task<hanet_transaction> AddTransactionHistoryLog(Hanet_Checkin_Data_History data)
        {
            try
            {
                var check = await _deviceAutoPushDbContext.hanet_transaction.FirstOrDefaultAsync(x => x.deviceID == data.deviceID && x.time == data.checkinTime);
                if (check != null)
                    return check;

                hanet_transaction hanet_Transaction = new hanet_transaction()
                {
                    id = Guid.NewGuid().ToString(),
                    content = JsonConvert.SerializeObject(data),
                    deviceID = data.deviceID,
                    time = data.checkinTime,
                    transaction_type = TransactionRealTime,
                    created_time = DateTime.Now,
                };
                _deviceAutoPushDbContext.Add(hanet_Transaction);
                _deviceAutoPushDbContext.SaveChanges();
                return hanet_Transaction;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return null;
            }
        }

    }
}
