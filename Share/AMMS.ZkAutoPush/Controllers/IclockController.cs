using EventBus.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using Shared.Core.Loggers;
using AMMS.ZkAutoPush.Data;
using Shared.Core.Caches.Redis;
using AMMS.DeviceData.RabbitMq;
using AMMS.ZkAutoPush.Applications.V1;
using AMMS.ZkAutoPush.Applications;

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
    private readonly StartupDataService _startupDataService;
    private readonly DeviceCacheService _deviceCacheService;
    private readonly DeviceCommandCacheService _deviceCommandCacheService;

    public IclockController(ILogger<IclockController> logger
        , IConfiguration configuration
        , IOptions<EventBusSettings> eventBusSettings
        , IEventBusAdapter eventBusAdapter
        , ICacheService cacheService
        , StartupDataService startupDataService
        , DeviceCacheService deviceCacheService
        , DeviceCommandCacheService deviceCommandCacheService
        )
    {
        _logger = logger;
        _configuration = configuration;
        _eventBusSettings = eventBusSettings.Value;
        _eventBusAdapter = eventBusAdapter;
        _cacheService = cacheService;
        _startupDataService = startupDataService;
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



            //string retValue = Retval_404;

            //if (Db.devMap.ContainsKey(sn))
            //{
            //    string registrycode = (string)Db.devMap[sn]["registrycode"];
            //    retValue = "RegistryCode=" + registrycode;

            //    string mes = $"\t has been registered, register code: {registrycode}";
            //    LogWarning(mes);
            //}
            //else
            //{
            //    string randomString = Util.GetRandomString(10);
            //    string datas = Util.GetStreamData(HttpContext.Request);
            //    Dictionary<string, string> dataMap = Util.ParseStringToMap(datas);

            //    dataMap["ServerVersion"] = "10.2";
            //    dataMap["ServerName"] = "myServerName";
            //    dataMap["PushVersion"] = "5.6";
            //    dataMap["ErrorDelay"] = "30";
            //    dataMap["RequestDelay"] = "3";
            //    dataMap["TransTimes"] = "00:00\t23:59";
            //    dataMap["TransInterval"] = "1";
            //    dataMap["TransTables"] = "User\tTransaction";
            //    dataMap["Realtime"] = "1";
            //    dataMap["SessionID"] = HttpContext.Session.Id;

            //    Dictionary<string, object> optionsMap = new Dictionary<string, object>
            //    {
            //        { "options", dataMap },
            //        { "registrycode", randomString }
            //    };

            //    Db.devMap[sn] = optionsMap;
            //    retValue = "RegistryCode=" + randomString;

            //    string mes = $"\t not registered, go to register, return register code: {randomString}";
            //    LogWarning(mes);
            //}

            //return retValue;

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
        Logger.Warning($"cdata: method: {Request.Method}, sn: {sn},  table: {table}");
        try
        {
            var req = HttpContext.Request;
            var contentType = req.ContentType;

            string content = "";
            using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 4096, true))
                content = reader.ReadToEndAsync().Result;
            //Logger.Warning(content);

            //Đẩy dữ liệu lên RabbitMQ
            ZK_TA_DATA data = new ZK_TA_DATA();
            data.ReceivedIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            data.Content = content;
            data.Table = table;
            data.SN = sn;
            data.ReceivedTime = DateTime.Now;

            var aa = await _eventBusAdapter.GetSendEndpointAsync(_configuration.GetValue<string>("DataArea") + EventBusConstants.ZK_Auto_Push_D2S);
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

        var thietBiUpdate = ZK_SV_PUSHService.ListTerminal.FirstOrDefault(o => o.sn == sn);
        //var thietBiUpdate = await _deviceCacheService.Get(sn);
        if (thietBiUpdate != null)
        {
            thietBiUpdate.last_activity = DateTime.Now;
            thietBiUpdate.isconnect = true;
            ////Cập nhật vào csdl
            //try
            //{
            //    await SaveDevice(thietBiUpdate);
            //}
            //catch (Exception e)
            //{

            //}
        }

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

        try
        {

            if (!ZK_SV_PUSHService.ListIclockCommand.Any())
                return Retval_OK;

            List<IclockCommand> xx = new List<IclockCommand>();
            if (ZK_SV_PUSHService.ListIclockCommand.Count > 0)
                xx = ZK_SV_PUSHService.ListIclockCommand.Where(o => o != null && o.SerialNumber == sn && !o.IsRequest).ToList();

            //var xx = await _deviceCommandCacheService.Gets(sn);

            if (xx == null || xx.Count() < 1)
            {
                return Retval_OK;
            }

            string commanText = "";
            int i = 0;
            int totalkb = 1000 * 1024;
            int contentkb = 0;
            while (i < 200 || contentkb < totalkb)
            {
                IclockCommand x = null;
                try
                {
                    if (xx != null && xx.Count > 0)
                    {
                        x = xx.Where(o => o != null).FirstOrDefault(o => o.SerialNumber == sn && !o.IsRequest);
                    }
                }
                catch (Exception e)
                {
                    Logger.Warning(e.Message);
                }
                if (x != null)
                {
                    int emlkb = Encoding.Unicode.GetByteCount(x.Command);
                    if (emlkb > totalkb)
                    {
                        if (x.DataTable == IclockDataTable.A2NguoiIclockUserPicSyn)
                        {
                            ZK_SV_PUSHService.ListIclockCommand.Remove(x);
                            //await _deviceCommandCacheService.Clear(x.DataId);
                        }
                        continue;
                    }
                    if (contentkb + emlkb < totalkb)
                    {
                        x.IsRequest = true;
                        x.CommitTime = DateTime.Now;

                        if (commanText == "")
                            commanText = x.Command;
                        else
                            commanText = commanText + "\n" + x.Command;

                        contentkb = Encoding.Unicode.GetByteCount(commanText);
                    }
                    else
                        break;
                }
                else
                    break;
                i++;
            }
            if (string.IsNullOrEmpty(commanText))
            {
                return "OK";
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
            Logger.Warning(content);
            if (string.IsNullOrEmpty(content))
                return Retval_OK;

            //Đẩy dữ liệu lên RabbitMQ
            ZK_DEVICE_RP data = new ZK_DEVICE_RP();
            data.ReceivedIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            data.Content = content;
            data.SN = sn;
            data.ReceivedTime = DateTime.Now;

            var aa = await _eventBusAdapter.GetSendEndpointAsync(_configuration.GetValue<string>("DataArea") + EventBusConstants.ZK_Response_Push_D2S);
            await aa.Send(data);

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
                            var x = ZK_SV_PUSHService.ListIclockCommand.FirstOrDefault(o => o.SerialNumber == sn && o.Id.ToString() == ID);
                            if (x != null)
                            {
                                x.RevicedTime = DateTime.Now;
                            }
                        }
                    }
                }
            }
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
