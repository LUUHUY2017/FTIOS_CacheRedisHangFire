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

namespace AMMS.Hanet.Applications.V1.Service
{
    /// <summary>
    /// Xử lý data
    /// </summary>
    public class HANET_Device_Reponse_Service
    {
        private readonly IEventBusAdapter _eventBusAdapter;

        DeviceAutoPushDbContext _deviceAutoPushDbContext;
        public HANET_Device_Reponse_Service(DeviceAutoPushDbContext deviceAutoPushDbContext, IEventBusAdapter eventBusAdapter)
        {
            _deviceAutoPushDbContext = deviceAutoPushDbContext;
            _eventBusAdapter = eventBusAdapter;
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
                var data = JsonConvert.DeserializeObject<Hanet_Checkin_Data>(reponse.data);

                if (data == null)
                {
                    return;
                }

                await AddTransactionLog(data, reponse);

                await AddATTLOG(data);

            }

        }

        private async Task AddATTLOG(Hanet_Checkin_Data data)
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
                        PersonCode = data.aliasID,
                        //PersonId = taData.PersonId,
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

                    var aa = await _eventBusAdapter.GetSendEndpointAsync(EventBusConstants.DataArea + EventBusConstants.Data_Auto_Push_D2S);
                    await aa.Send(rB_Response);


                    if (string.IsNullOrEmpty(data.detected_image_url))
                    {
                        return;
                    }

                    await AddATTImage(data);
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

        private async Task AddATTImage(Hanet_Checkin_Data data)
        {
            try
            {
                var image = ConvertImage(data.detected_image_url, data.id + ".jpg", ImageFormat.Jpeg);


                TA_AttendenceImage tA_AttendenceImage = new TA_AttendenceImage()
                {
                    Id = Guid.NewGuid().ToString(),
                    ImageBase64 = image,
                    PersonCode = data.aliasID,
                    SerialNumber = data.deviceID,
                    TimeEvent = DateTime.Now,
                };

                RB_DataResponse rB_DataResponse = new RB_DataResponse()
                {
                    Id = tA_AttendenceImage.Id,
                    Content = JsonConvert.SerializeObject(tA_AttendenceImage),
                    ReponseType = RB_DataResponseType.AttendenceImage,
                };

                var aa = await _eventBusAdapter.GetSendEndpointAsync(EventBusConstants.DataArea + EventBusConstants.Data_Auto_Push_D2S);
                await aa.Send(rB_DataResponse);
            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
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
                    deviceID = data.deviceID
                };
                _deviceAutoPushDbContext.Add(hanet_Transaction);
                _deviceAutoPushDbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}
