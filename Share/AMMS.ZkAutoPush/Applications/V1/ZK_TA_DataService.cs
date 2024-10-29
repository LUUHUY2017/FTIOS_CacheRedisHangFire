using AMMS.DeviceData.RabbitMq;
using AMMS.ZkAutoPush.Data;
using AMMS.ZkAutoPush.Datas.Databases;
using AMMS.ZkAutoPush.Datas.Entities;
using EventBus.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Core.Loggers;
using System.Drawing;
using System.Text;

namespace AMMS.ZkAutoPush.Applications.V1;
public class ZK_TA_DataService
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly IConfiguration _configuration;

    private readonly DeviceAutoPushDbContext _deviceAutoPushDbContext;
    public ZK_TA_DataService(DeviceAutoPushDbContext deviceAutoPushDbContext, IEventBusAdapter eventBusAdapter, IConfiguration configuration)
    {
        _deviceAutoPushDbContext = deviceAutoPushDbContext;
        _eventBusAdapter = eventBusAdapter;
        _configuration = configuration;
    }
    private const string ATTLOG = "ATTLOG";
    private const string ATTPHOTO = "ATTPHOTO";
    private const string OPERLOG = "OPERLOG";

     public async Task AddTA_Data(ZK_TA_DATA data)
    {
        try
        {
            Logger.Warning(data.Content);
            if (data.Table == ATTLOG)
                await AddATTLOG(data);
            else if (data.Table == ATTPHOTO)
                await AddATTPHOTO(data);

        }
        catch (Exception)
        {

            throw;
        }
    }

    private async Task AddATTLOG(ZK_TA_DATA data)
    {
        try
        {
            string content = data.Content;
            if (string.IsNullOrEmpty(content))
                return;

            zk_transaction taData = new zk_transaction();
            taData.Id = Guid.NewGuid().ToString();
            taData.IpAddress = data.ReceivedIp;
            taData.SerrialNumber = data.SN;
            taData.CreatedTime = data.ReceivedTime;
            taData.Content = content;
            StringReader strReader = new StringReader(content);
            string line = strReader.ReadLine();
            if (!string.IsNullOrEmpty(line))
            {
                if (line.Contains("\\t"))
                    line = line.Replace("\\t", " ");
                if (line.Contains("\t"))
                    line = line.Replace("\t", " ");
                var content_arr = line.Split(' ');
                if (content_arr != null && content_arr.Length > 9) // && content_arr[3] == "4"
                {
                    string manguoi = content_arr[0];
                    DateTime timeAtt = DateTime.Parse(content_arr[1] + " " + content_arr[2]);
                    try
                    {
                        //Lấy thông tin nguoi
                        var nguoi = _deviceAutoPushDbContext.zk_user.FirstOrDefault(x => x.user_code == manguoi);
                        //Lấy thông tin thiết bị
                        var thietbi = _deviceAutoPushDbContext.zk_terminal.FirstOrDefault(x => x.sn == data.SN);
                        taData.PersonCode = manguoi;
                        taData.PersonId = nguoi?.Id;
                        taData.DeviceId = thietbi?.Id;
                        taData.TimeEvent = timeAtt;

                        //Lưu vào CSDL
                        await _deviceAutoPushDbContext.zk_transaction.AddAsync(taData);
                        var save = await _deviceAutoPushDbContext.SaveChangesAsync();

                    }
                    catch (Exception e)
                    {
                        Logger.Warning(e.Message);
                    }
                }
            }

            #region Gửi lên RBMQ
            try
            {
                TA_AttendenceHistory tA_AttendenceHistory = new TA_AttendenceHistory()
                {
                    Id = taData.Id,
                    DeviceId = taData.DeviceId,
                    PersonCode = taData.PersonCode,
                    PersonId = taData.PersonId,
                    SerialNumber = taData.SerrialNumber,
                    TimeEvent = taData.TimeEvent,
                    Type = taData.VerifyType,
                };

                RB_DataResponse rB_Response = new RB_DataResponse()
                {
                    Id = tA_AttendenceHistory.Id,
                    Content = JsonConvert.SerializeObject(tA_AttendenceHistory),
                    ReponseType = RB_DataResponseType.AttendenceHistory,
                };

                var aa = await _eventBusAdapter.GetSendEndpointAsync(EventBusConstants.DataArea + EventBusConstants.Data_Auto_Push_D2S);
                await aa.Send(rB_Response);

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

    private async Task AddATTPHOTO(ZK_TA_DATA data)
    {
        try
        {
            string content = data.Content;
            if (string.IsNullOrEmpty(content))
                return;
            StringReader strReader = new StringReader(content);
            string PIN = strReader.ReadLine();
            string img_filename = GetKeyValue(PIN, '=')[1];
            string SN = strReader.ReadLine();
            string strsize = strReader.ReadLine();
            string size = GetKeyValue(strsize, '=')[1];
            string userId = GetKeyValue(img_filename, '-')[1].Replace(".jpg", "");
            string uploadPhoto = strReader.ReadToEnd();
            string uploadPhotoData = uploadPhoto.Split('=')[1].Replace("uploadphoto", "");
            // var s  = System.Text.Encoding.ASCII.GetString(System.Text.Encoding.ASCII.GetBytes(uploadPhotoData));

            #region Gửi lên RBMQ
            try
            {
                var nguoi = _deviceAutoPushDbContext.zk_user.FirstOrDefault(x => x.user_code == userId);

                TA_AttendenceImage tA_AttendenceImage = new TA_AttendenceImage()
                {
                    Id = Guid.NewGuid().ToString(),
                    ImageBase64 = "",
                    PersonId = nguoi?.Id,
                    PersonCode = userId,
                    SerialNumber = SN,
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
    private string[] GetKeyValue(string content, char spl)
    {
        string[] arr = new string[2];
        arr[0] = "";
        arr[1] = "";
        if (!string.IsNullOrEmpty(content))
        {
            try
            {
                var content_arr = content.Split(spl);
                if (content_arr != null && content_arr.Length == 2)
                {
                    arr[0] = content_arr[0];
                    arr[1] = content_arr[1];
                }
            }
            catch
            {

            }
        }
        return arr;
    }
    private string ConvertToBase64(byte[] imageBytes)
    {
        using (var ms = new MemoryStream(imageBytes))
        {
            using (var img = Image.FromStream(ms))
            {
                using (var jpgStream = new MemoryStream())
                {
                    img.Save(jpgStream, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] jpgBytes = jpgStream.ToArray();
                    return Convert.ToBase64String(jpgBytes);
                }
            }
        }
    }
}
