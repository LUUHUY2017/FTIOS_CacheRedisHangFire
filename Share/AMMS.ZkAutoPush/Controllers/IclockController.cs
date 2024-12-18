﻿using EventBus.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using Shared.Core.Loggers;
using AMMS.ZkAutoPush.Data;
using Shared.Core.Caches.Redis;
using AMMS.DeviceData.RabbitMq;
using AMMS.ZkAutoPush.Applications.V1;
using AMMS.ZkAutoPush.Applications;
using MassTransit.Monitoring.Performance;

namespace AMMS.ZkAutoPush.Controllers;

[ApiController]
[Route("[controller]")]
public class IclockController : ControllerBase
{
    const string Retval_404 = "404";
    const string Retval_OK = "OK";


    private readonly ILogger<IclockController> _logger;
    //Uri _uri;
    private readonly EventBusSettings _eventBusSettings;
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly IConfiguration _configuration;
    private readonly ICacheService _cacheService;
    private readonly DeviceCacheService _deviceCacheService;
    private readonly DeviceCommandCacheService _deviceCommandCacheService;

    public IclockController(ILogger<IclockController> logger
        , IConfiguration configuration
        , IOptions<EventBusSettings> eventBusSettings
        , IEventBusAdapter eventBusAdapter
        , ICacheService cacheService
         , DeviceCacheService deviceCacheService
        , DeviceCommandCacheService deviceCommandCacheService
        )
    {
        _logger = logger;
        _configuration = configuration;
        _eventBusSettings = eventBusSettings.Value;
        _eventBusAdapter = eventBusAdapter;
        _cacheService = cacheService;
        _deviceCacheService = deviceCacheService;
        _deviceCommandCacheService = deviceCommandCacheService;
    }
    /// <summary>
    /// Khởi tạo
    /// </summary>
    /// <param name="sn"></param>
    /// <param name="pushver"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    [HttpGet("cdata")]
    public async Task<string> cdata(string sn, string pushver, string options)
    {
        Logger.Warning($"cdata: method: {Request.Method}, sn: {sn}");
        try
        {
            var thietbi = await _deviceCacheService.Get(sn);
            //var thietbi = _startupDataService.GetDevice(sn);
            string sessionId = HttpContext.Session.Id;
            string randomString = "1234567890"; //Util.GetRandomString(10);

            var retVal = "registry=ok\n"
                                   + $"RegistryCode={randomString}\n"
                                   + "ServerVersion=3.1.2\n"
                                   + "ServerName=ADMS\n"
                                   + $"PushProtVer={pushver}\n"
                                   + "ErrorDelay=60\n" //Interval time for the client to reconnect to the server after networking connection failure. The recommended value is 30 to 300s.
                                   + "RequestDelay=60\n" //The interval (in seconds) at which the client sends the command acquiring request. If no value is configured at the client, the default value 30s are used.
                                   + "TransTimes=00:00;23:59\n" //Time at which the client checks for and transmits new data regularly (in a 24 - hour format: hour: minute) and multiple times are separated by semicolons.Up to 10 times are supported.For example, TransTimes = 00: 00; 14: 00
                                   + "TransInterval=2\n" //The interval (in minutes) at which the system checks whether any new data needs to be transmitted. If no value is configured at the client, the default value 2 min is used.
                                                         //+ "TransTables=\n" //The new data that needs to be checked and uploaded. The default value is User Transaction, which means the user and access control records need to be automatically uploaded
                                   + $"SessionID={sessionId}\n"

                                   //+ "\nStamp=664562654"
                                   //+ "\nOpStamp=664571571"
                                   + "PhotoStamp=664562654\n"

                                   + "EncryptFlag=1000000000\n"
                                   + "PushOptionsFlag=1\n"
                                   + "SupportPing=1\n"
                                   + "PushOptions=UserCount,TransactionCount,FingerFunOn,FPVersion,FPCount,FaceFunOn,FaceVersion,FaceCount,FvFunOn,FvVersion,FvCount,PvFunOn,PvVersion,PvCount,BioPhotoFun,BioDataFun,PhotoFunOn,~LockFunOn\n"
                                   + "TransFlag=TransData AttLog OpLog   AttPhoto EnrollFP    EnrollUser FPImag  ChgUser ChgFP   FACE UserPic FVEIN BioPhoto\n"
                                   + "TimeZone=7\n"
                                   + "Realtime=1\n"
                                   + "Encrypt=0\n"
                                   ;

            return retVal;




        }
        catch (Exception e)
        {
            Logger.Error(e.Message);
            Logger.Error(e.StackTrace);

            return Retval_404;
        }
    }
    /// <summary>
    /// Dữ liệu chấm công, log thiết bị trả về
    /// </summary>
    /// <param name="sn"></param>
    /// <param name="table"></param>
    /// <returns></returns>
    [HttpPost("cdata")]
    public async Task<string> cdata(string sn, string table)
    {
        Logger.Warning($"cdata: method: {Request.Method}, sn: {sn}");

        try
        {
            var req = HttpContext.Request;
            var contentType = req.ContentType;

            string content = "";
            using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 4096, true))
                content = reader.ReadToEndAsync().Result;
            //Logger.Warning($"cdata: method: {Request.Method}, sn: {sn},  table: {table}, content: {content}");

            //Đẩy dữ liệu lên RabbitMQ
            ZK_TA_DATA data = new ZK_TA_DATA();
            data.ReceivedIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            data.Content = content;
            data.Table = table;
            data.SN = sn;
            data.ReceivedTime = DateTime.Now;

            var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}_{EventBusConstants.ZKTECO}{EventBusConstants.ZK_Auto_Push_D2S}");
            await aa.Send(data);
            //var a = await _cacheService.GetData<string>($"{sn}");

            return Retval_OK;
        }
        catch (Exception e)
        {
            Logger.Error(e.Message);
            Logger.Error(e.StackTrace);
            return Retval_404;
        }
    }

    /// <summary>
    /// Thiết bị gửi yêu cầu để kiểm tra kết nối
    /// </summary>
    /// <param name="sn"></param>
    /// <returns></returns>
    [HttpGet("ping")]
    public async Task<string> ping(string sn)
    {
        Logger.Warning($"ping: method: {Request.Method}, sn: {sn}");
        //Xử lý thiết bị khi trả ping về
        return Retval_OK;
    }

    /// <summary>
    /// Thiết bị lấy request từ hệ thống
    /// </summary>
    /// <param name="sn"></param>
    /// <returns></returns>
    [HttpGet("getrequest")]
    public async Task<string> getrequest(string sn)
    {
        Logger.Warning($"getrequest: method: {Request.Method}, sn: {sn}");
        //Kiểm tra trạng thái thiết bị
        try
        {
            var thietBiUpdate = await _deviceCacheService.Get(sn);
            if (thietBiUpdate != null)
            {
                thietBiUpdate.last_activity = DateTime.Now;
                //Thiết bị online
                if (thietBiUpdate.online_status == false)
                {
                    thietBiUpdate.time_online = DateTime.Now;
                    thietBiUpdate.online_status = true;
                    //Lấy lại lệnh sau khi online
                    await _deviceCommandCacheService.AddCommandNotRuning(sn);
                }
                await _deviceCacheService.Save(thietBiUpdate);

            }

        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }

        try
        {
            //Tìm danh sách lệnh của thiết bị
            List<IclockCommand> listAll = await _deviceCommandCacheService.Gets(sn);
            if (listAll == null || listAll.Count < 0)
                return Retval_OK;
            //Các lệnh chưa thực hiện
            var listUsing = listAll.Where(o => o != null && o.SerialNumber == sn && !o.IsRequest).ToList();

            if (listUsing == null || listUsing.Count() < 1)
            {
                return Retval_OK;
            }

            string commanText = "";
            int i = 0;
            int totalkb = 1000 * 1024;
            int contentkb = 0;
            while (i < 200 || contentkb < totalkb)
            {
                IclockCommand currentCommand = null;
                try
                {
                    if (listUsing != null && listUsing.Count > 0)
                    {
                        currentCommand = listUsing.Where(o => o != null).OrderBy(m => m.Id).FirstOrDefault(o => o.SerialNumber == sn && !o.IsRequest);
                    }
                }
                catch (Exception e)
                {
                    Logger.Warning(e.Message);
                }
                if (currentCommand != null)
                {
                    int emlkb = Encoding.Unicode.GetByteCount(currentCommand.Command);
                    //Kiểm tra nếu số kb của lệnh lớn hơn quy định
                    if (emlkb > totalkb)
                    {
                        if (currentCommand.DataTable == IclockDataTable.A2NguoiIclockUserPicSyn)
                        {
                            Logger.Warning("Ảnh lớn hơn quy định " + currentCommand.SerialNumber + " " + currentCommand.Id);
                            await _deviceCommandCacheService.Remove(sn, currentCommand.Id.ToString());
                            listUsing.Remove(currentCommand);
                        }
                        continue;
                    }
                    if (contentkb + emlkb < totalkb)
                    {
                        currentCommand.IsRequest = true;
                        currentCommand.CommitTime = DateTime.Now;
                        //Lưu lại thông tin vào caches
                        await _deviceCommandCacheService.Save(currentCommand);

                        //Thêm lệnh vào chuỗi lệnh trả về
                        if (commanText == "")
                            commanText = currentCommand.Command;
                        else
                            commanText = commanText + "\n" + currentCommand.Command;

                        contentkb = Encoding.Unicode.GetByteCount(commanText);
                    }
                    else
                        break;
                }
                else
                    break;
                i++;
            }

            Console.WriteLine("So ban ghi da gui: " + i.ToString());

            if (string.IsNullOrEmpty(commanText))
            {
                return Retval_OK;
            }

            return commanText;


        }
        catch (Exception e)
        {
            Logger.Error(e);
            return Retval_404;
        }
    }
    /// <summary>
    /// Thiết bị đẩy lại trạng thái lệnh thực hiện
    /// </summary>
    /// <param name="sn"></param>
    /// <returns></returns>
    [HttpPost("devicecmd")]
    public async Task<string> devicecmd(string sn)
    {
        Logger.Warning($"devicecmd: method: {Request.Method}, sn: {sn}");

        try
        {
            var req = HttpContext.Request;
            var contentType = req.ContentType;

            string content = "";
            using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 4096, true))
                content = reader.ReadToEndAsync().Result;
            if (string.IsNullOrEmpty(content))
                return Retval_OK;

            //Đẩy dữ liệu lên RabbitMQ
            ZK_DEVICE_RP data = new ZK_DEVICE_RP();
            data.ReceivedIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            data.Content = content;
            data.SN = sn;
            data.ReceivedTime = DateTime.Now;

            var contentArr1 = content.Split('\n');
            if (contentArr1.Length > 0)
            {
                foreach (var elm in contentArr1)
                {

                    if (!string.IsNullOrEmpty(elm))
                    {
                        var contentArr = elm.Split('&');
                        if (contentArr.Length == 3)
                        {
                            var ID = contentArr[0].Split('=')[1];
                            var x = await _deviceCommandCacheService.GetByCode(sn, ID);
                            if (x != null)
                            {
                                x.RevicedTime = DateTime.Now;
                                await _deviceCommandCacheService.Save(x);
                            }
                        }
                    }
                }
            }

            var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}_{EventBusConstants.ZKTECO}{EventBusConstants.ZK_Response_Push_D2S}");
            await aa.Send(data);

        }
        catch (Exception e)
        {
            Logger.Error(e.Message);
            Logger.Error(e.StackTrace);

            return Retval_404;

        }
        return Retval_OK;
    }


}
