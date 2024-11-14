using AMMS.DeviceData.RabbitMq;
using AMMS.ZkAutoPush.Datas.Databases;
using AMMS.ZkAutoPush.Datas.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using Shared.Core.Loggers;
using System.ComponentModel.DataAnnotations;
using static AMMS.ZkAutoPush.Applications.CronJobs.CronJobService;

namespace AMMS.ZkAutoPush.Applications.V1
{
    public class StartupDataService
    {
        private readonly IConfiguration _configuration;
        private readonly DeviceCacheService _deviceCacheService;

        private readonly DeviceAutoPushDbContext _deviceAutoPushDbContext;
        public StartupDataService(DeviceAutoPushDbContext deviceAutoPushDbContext, IConfiguration configuration, DeviceCacheService deviceCacheService)
        {
            _deviceAutoPushDbContext = deviceAutoPushDbContext;
            _configuration = configuration;
            _deviceCacheService = deviceCacheService;

        }
        public static AccessTokenServer accessToken { get; set; }


        public async Task LoadConfigData()
        {

            var serveraddress = _configuration.GetValue<string>("AuthenticationApi:Authority");
            if (!string.IsNullOrEmpty(serveraddress))
            {
                serverMater = serveraddress;
            }
            var clientid = _configuration.GetValue<string>("AuthenticationApi:ClientId");
            if (!string.IsNullOrEmpty(clientid))
            {
                client_id = clientid;
            }
            var clientsecret = _configuration.GetValue<string>("AuthenticationApi:ClientSecret");
            if (!string.IsNullOrEmpty(clientsecret))
            {
                client_secret = clientsecret;
            }
            await GetListDevice();

            var terminals = await _deviceAutoPushDbContext.zk_terminal.ToListAsync();

            foreach (var terminal in terminals)
            {
                terminal.online_status = false;
                terminal.last_activity = DateTime.Now;
                await _deviceCacheService.Save(terminal);
            }
        }

        public static string serverMater { get; set; }
        public static string client_id { get; set; }
        public static string client_secret { get; set; }


        public static AccessTokenServer Get_Token()
        {
            try
            {
                if (string.IsNullOrEmpty(serverMater))
                    return null;
                var options = new RestClientOptions($"{serverMater}")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("connect/token", Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("client_id", client_id);
                request.AddParameter("client_secret", client_secret);
                request.AddParameter("grant_type", "client_credentials");
                RestResponse response = client.Execute(request);

                if (response.IsSuccessStatusCode)
                {
                    accessToken = JsonConvert.DeserializeObject<AccessTokenServer>(response.Content);
                    if (accessToken == null)
                    {
                        throw new Exception("Can't get access token");
                    }
                }
                else
                    throw new Exception(response.ErrorMessage);
                return accessToken;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static AccessTokenServer Reset_Token()
        {
            try
            {
                if (string.IsNullOrEmpty(serverMater))
                    return null;
                var options = new RestClientOptions($"{serverMater}")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("api/v1/Authentication/RefreshToken", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                var body = @"{"
               + "client_id:'" + client_id + "',"
               + "client_secret:'" + client_secret + "',"
               + "grant_type:'refresh_token',"
               + "client_secret:'" + accessToken.refresh_token + "',"
                + "}";
                request.AddStringBody(body, DataFormat.Json);

                RestResponse response = client.Execute(request);
                accessToken = JsonConvert.DeserializeObject<AccessTokenServer>(response.Content);
                if (accessToken == null)
                {
                    throw new Exception("Can't get access token");
                }
                return accessToken;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static AccessTokenServer Check_Token()
        {
            try
            {
                if (accessToken == null || accessToken.access_token == "" || accessToken.access_token == null)
                {
                    Get_Token();
                }
                if (accessToken == null)
                    return null;

                if (accessToken.expires_time < DateTime.Now)
                {
                    Reset_Token();
                }
                return accessToken;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Post data
        /// </summary>
        /// <param name="api"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> PostData(string api, Object data)
        {
            try
            {
                Check_Token();
                if (accessToken == null || accessToken.access_token == "" || accessToken.access_token == null)
                    return "";

                var jsonObj = JsonConvert.SerializeObject(data);
                var options = new RestClientOptions($"{serverMater}")
                {
                    MaxTimeout = -1,
                    FollowRedirects = false,
                };
                var client = new RestClient(options);
                var request = new RestRequest(api, Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + accessToken.access_token);
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);
                RestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                    return response.Content;
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string GetData(string api)
        {
            try
            {
                Check_Token();
                if (accessToken == null || accessToken.access_token == "" || accessToken.access_token == null)
                    return "";
                if (string.IsNullOrEmpty(serverMater))
                    return "";
                var options = new RestClientOptions($"{serverMater}")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest(api, Method.Get);
                request.AddHeader("Authorization", "Bearer " + accessToken.access_token);
                RestResponse response = client.Execute(request);
                var strJson = response.Content;
                if (response.IsSuccessful)
                    return strJson;
                else
                    return "";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Lấy danh sách thiết bị
        /// </summary>
        /// <returns></returns>
        public async Task GetListDevice()
        {
            try
            {
                // string url = "api/v1/Device/GetsForDeviceModel?deviceModel=hanet";
                string url = "api/v1/Device/Gets";

                var data = GetData(url);
                if (string.IsNullOrEmpty(data))
                    return;

                var returnData = JsonConvert.DeserializeObject<ServerResponse>(data);

                if (returnData == null || returnData.data == null)
                {
                    return;
                }

                List<A2_Device> devices = returnData.data.Where(x => x.DeviceModel != null && x.DeviceModel.ToUpper() == EventBusConstants.ZKTECO).ToList();
                foreach (var device in devices)
                {
                    var terminal = new zk_terminal
                    {
                        Id = device.Id,
                        sn = device.SerialNumber,
                        name = device.DeviceName,
                        ip_address = device.IPAddress,
                    };
                    await SaveDevice(terminal);

                }

            }
            catch (Exception ex)
            {

                Logger.Error(ex.Message);
            }
        }
        /// <summary>
        /// Cập nhật trạng thái thiết bị
        /// </summary>
        /// <returns></returns>
        public async Task UpdateStatus(List<terminal_status> data)
        {
            try
            {
                string url = "api/v1/MonitorDevice/ChangeStatusDevice";

                var result = await PostData(url, data);

                return;

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// Lưu thông tin thiết bị
        /// </summary>
        /// <param name="a2_Devices"></param>
        /// <returns></returns>
        public async Task SaveDevice(zk_terminal a2_Devices)
        {
            try
            {
                var obj = _deviceAutoPushDbContext.zk_terminal.FirstOrDefault(x => x.sn == a2_Devices.sn);
                if (obj == null)
                {
                    a2_Devices.create_time = DateTime.Now;
                    await _deviceAutoPushDbContext.zk_terminal.AddAsync(a2_Devices);
                    await _deviceAutoPushDbContext.SaveChangesAsync();

                }
                else
                {

                    obj.name = a2_Devices.name;
                    await _deviceAutoPushDbContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

    } /// <summary>
      /// Dữ liệu SV trả về
      /// </summary>
    public class ServerResponse
    {
        public int code { get; set; }
        public bool succeeded { get; set; }
        public string message { get; set; }
        public List<A2_Device> data { get; set; }

    }

    public class AccessTokenServer
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }

        int _expires_in { get; set; }
        public int expires_in { get { return _expires_in; } set { _expires_in = value; expires_time = DateTime.Now.AddSeconds(value); } }
        public string token_type { get; set; }
        public string scope { get; set; }

        /// <summary>
        /// UTC 0
        /// </summary>

        public DateTime expires_time { get; private set; }

    }

    public class A2_Device
    {
        public string Id { get; set; }
        public string? DeviceCode { get; set; }
        public string? DeviceName { get; set; }
        public string? DeviceParam { get; set; }
        public string? DeviceInfo { get; set; }

        public string? IPAddress { get; set; }
        public string? KeyLicense { get; set; }
        public string? KeyConnect { get; set; }
        public string? DeviceType { get; set; }

        [MaxLength(500)]
        public string? DeviceDescription { get; set; }
        public string? Param { get; set; }
        [MaxLength(50)]
        public string? SerialNumber { get; set; }
        public string? MacAddress { get; set; }

        public int? HTTPPort { get; set; }
        public int? PortConnect { get; set; }
        public string? LaneId { get; set; }
        public string? GateId { get; set; }
        public DateTime? ConnectUpdateTime { get; set; }

        [MaxLength(50)]
        public string? ActiveKey { get; set; }
        public string? DeviceID { get; set; }
        public string? Password { get; set; }
        public string? BrandName { get; set; }

        public bool? ConnectionStatus { get; set; }
        /// <summary>
        /// Tín hiệu vào true - false
        /// </summary>
        public bool? DeviceIn { get; set; }
        /// <summary>
        /// Tín hiệu ra true - false
        /// </summary>
        public bool? DeviceOut { get; set; }
        /// <summary>
        /// Tín hiệu ra dạng chuỗi
        /// </summary>
        public bool? DeviceReader { get; set; }
        /// <summary>
        /// Loại thiết bị vào true
        /// </summary>
        public bool? DeviceInput { get; set; }

        /// <summary>
        /// Model thiết bị  
        /// </summary>
        public string? DeviceModel { get; set; }

    }
}
