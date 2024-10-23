using EventBus.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using Shared.Core.Loggers;
using AMMS.ZkAutoPush.Data;
using Shared.Core.Caches.Redis;
using AMMS.DeviceData.RabbitMq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

    public IclockController(ILogger<IclockController> logger
        , IConfiguration configuration
        , IOptions<EventBusSettings> eventBusSettings
        , IEventBusAdapter eventBusAdapter
        , ICacheService cacheService
        )
    {
        _logger = logger;
        _configuration = configuration;
        _eventBusSettings = eventBusSettings.Value;
        _eventBusAdapter = eventBusAdapter;
        _cacheService = cacheService;
    }
    /// <summary>
    /// Khởi tạo
    /// </summary>
    /// <param name="sn"></param>
    /// <param name="pushver"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    [HttpGet("cdata")]
    public string cdata(string sn, string pushver, string options)
    {
        Logger.Warning($"cdata: method: {Request.Method}, sn: {sn}");
        try
        {

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
            Logger.Warning(content);

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
        return Retval_OK;
    }

    /// <summary>
    /// Thiết bị lấy request từ hệ thống
    /// </summary>
    /// <param name="sn"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("getrequest")]
    public string getrequest(string sn)
    {
        try
        {

        }
        catch (Exception)
        {

            throw;
        }
        return Retval_OK;
    }
    /// <summary>
    /// Thiết bị đẩy lại trạng thái lệnh thực hiện
    /// </summary>
    /// <param name="sn"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("devicecmd")]
    public async Task<string> devicecmd(string sn)
    {
        try
        {
            var req = HttpContext.Request;
            var contentType = req.ContentType;

            string content = "";
            using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 4096, true))
                content = reader.ReadToEndAsync().Result;
            Logger.Warning(content);

            //Đẩy dữ liệu lên RabbitMQ
            ZK_TA_DATA data = new ZK_TA_DATA();
            data.ReceivedIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            data.Content = content;
            data.SN = sn;
            data.ReceivedTime = DateTime.Now;
            var aa = await _eventBusAdapter.GetSendEndpointAsync(_configuration.GetValue<string>("DeviceCMDArea") + EventBusConstants.ZK_Response_Push_D2S);
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
