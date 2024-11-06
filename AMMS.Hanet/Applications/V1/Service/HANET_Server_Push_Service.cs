using AMMS.DeviceData.RabbitMq;
using AMMS.Hanet.Data;
using AMMS.Hanet.Datas.Databases;
using AMMS.Hanet.Datas.Entities;
using EventBus.Messages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using Shared.Core.Loggers;
using System.Text.Json.Serialization;

namespace AMMS.Hanet.Applications.V1.Service
{
    public class HANET_Server_Push_Service
    {
        private readonly IEventBusAdapter _eventBusAdapter;
        private readonly DeviceCacheService _deviceCacheService;
        public static string ServerHanet = "https://partner.hanet.ai";

        DeviceAutoPushDbContext _deviceAutoPushDbContext;
        public HANET_Server_Push_Service(DeviceAutoPushDbContext deviceAutoPushDbContext, IEventBusAdapter eventBusAdapter, DeviceCacheService deviceCacheService)
        {
            _deviceAutoPushDbContext = deviceAutoPushDbContext;
            _eventBusAdapter = eventBusAdapter;
            _deviceCacheService = deviceCacheService;
        }

        #region Pust data user
        public async Task ProcessDataServerPush(RB_ServerRequest rB_ServerRequest)
        {
            try
            {
                Logger.Warning("Data :" + rB_ServerRequest.RequestType + " - " + rB_ServerRequest.Action);

                //Tạo lệnh
                hanet_commandlog conmandlog = new hanet_commandlog()
                {
                    create_time = DateTime.Now,
                    Id = rB_ServerRequest.Id,
                    terminal_sn = rB_ServerRequest.SerialNumber,
                    terminal_id = rB_ServerRequest.DeviceId,
                    content = rB_ServerRequest.RequestParam,
                    command_ation = rB_ServerRequest.Action,
                    command_type = rB_ServerRequest.RequestType,
                    successed = false,
                };

                if (rB_ServerRequest.Action == ServerRequestAction.ActionAdd && rB_ServerRequest.RequestType == ServerRequestType.UserInfo)
                {
                    TA_PersonInfo? data = JsonConvert.DeserializeObject<TA_PersonInfo>(rB_ServerRequest.RequestParam);

                    if (data == null)
                        return;
                    Hanet_User user = new Hanet_User()
                    {
                        base64file = data.UserFace,
                        name = data.FullName,
                        aliasID = data.PersonCode,
                        //placeID = rB_ServerRequest.SchoolId,
                        placeID = HanetParam.PlateId,
                    };

                    if (await CheckUser(user))
                    {
                        var result = await UpdateFaceToHanet(user);
                        if (result != null)
                        {
                            conmandlog.return_time = DateTime.Now;
                            conmandlog.return_content = result.returnMessage;
                            conmandlog.return_value = result.returnCode;
                            if (result.returnCode == Hanet_Response_Static.SUCCESSCode)
                                conmandlog.successed = true;
                            else
                                conmandlog.successed = false;


                        }
                        else
                        {
                            conmandlog.return_time = DateTime.Now;
                            conmandlog.return_content = "Không kết nối được server hanet";
                            conmandlog.return_value = null;
                            conmandlog.successed = false;

                        }
                    }
                    else
                    {

                        var result = await AddUserToHanet(user);
                        if (result != null)
                        {
                            conmandlog.return_time = DateTime.Now;
                            conmandlog.return_content = result.returnMessage;
                            conmandlog.return_value = result.returnCode;
                            conmandlog.successed = true;

                        }
                        else
                        {
                            conmandlog.return_time = DateTime.Now;
                            conmandlog.return_content = "Không kết nối được server hanet";
                            conmandlog.return_value = null;
                            conmandlog.successed = false;

                        }
                    }
                    await AddCommand(conmandlog);
                }
                else if (rB_ServerRequest.Action == ServerRequestAction.ActionDelete && rB_ServerRequest.RequestType == ServerRequestType.UserInfo)
                {
                    TA_PersonInfo? data = JsonConvert.DeserializeObject<TA_PersonInfo>(rB_ServerRequest.RequestParam);

                    if (data == null)
                        return;
                    Hanet_User user = new Hanet_User()
                    {
                        base64file = data.UserFace,
                        name = data.FullName,
                        aliasID = data.PersonCode,
                        //placeID = rB_ServerRequest.SchoolId,
                        placeID = HanetParam.PlateId,
                    };
                    if (await RemoveUser(user))
                    {
                        conmandlog.return_time = DateTime.Now;
                        conmandlog.return_content = "Thành công";
                        conmandlog.return_value = 1;
                        conmandlog.successed = true;
                    }
                    else
                    {
                        conmandlog.return_time = DateTime.Now;
                        conmandlog.return_content = "Không thành công";
                        conmandlog.return_value = 0;
                        conmandlog.successed = false;
                    }


                    await AddCommand(conmandlog);

                }
                else if (rB_ServerRequest.Action == ServerRequestAction.ActionAdd && rB_ServerRequest.RequestType == ServerRequestType.Device)
                {
                    TA_Device? data = JsonConvert.DeserializeObject<TA_Device>(rB_ServerRequest.RequestParam);
                    if (data == null)
                        return;
                    if (await SaveDevice(data))
                    {
                        conmandlog.return_time = DateTime.Now;
                        conmandlog.return_content = "Thành công";
                        conmandlog.return_value = 1;
                        conmandlog.successed = true;

                        //Lưu thông tin caches
                        var terminal = await _deviceAutoPushDbContext.hanet_terminal.FirstOrDefaultAsync(x => x.Id == data.Id);
                        if (terminal != null)
                            await _deviceCacheService.Save(terminal);
                    }
                    else
                    {
                        conmandlog.return_time = DateTime.Now;
                        conmandlog.return_content = "Không thành công";
                        conmandlog.return_value = 0;
                        conmandlog.successed = false;
                    }
                    await AddCommand(conmandlog);

                }
                else if (rB_ServerRequest.Action == ServerRequestAction.ActionDelete && rB_ServerRequest.RequestType == ServerRequestType.Device)
                {
                    TA_Device? data = JsonConvert.DeserializeObject<TA_Device>(rB_ServerRequest.RequestParam);
                    if (data == null)
                        return;
                    if (await RemoveDevice(data))
                    {
                        conmandlog.return_time = DateTime.Now;
                        conmandlog.return_content = "Thành công";
                        conmandlog.return_value = 1;
                        conmandlog.successed = true;

                        //Xoá thông tin caches
                        await _deviceCacheService.Remove(data.SerialNumber);

                    }
                    else
                    {
                        conmandlog.return_time = DateTime.Now;
                        conmandlog.return_content = "Không thành công";
                        conmandlog.return_value = 0;
                        conmandlog.successed = false;
                    }
                    await AddCommand(conmandlog);

                }

                //Trả lại lệnh cho máy chủ
                RB_ServerResponse response = new RB_ServerResponse()
                {
                    Action = conmandlog.command_ation,
                    Content = conmandlog.return_content,
                    Id = conmandlog.Id,
                    RequestId = conmandlog.Id,
                    IsSuccessed = conmandlog.successed,
                    DateTimeResponse = conmandlog.return_time,
                    Message = conmandlog.successed == true ? RB_ServerResponseMessage.Complete : RB_ServerResponseMessage.InComplete,
                };

                var aa = await _eventBusAdapter.GetSendEndpointAsync(EventBusConstants.DataArea + EventBusConstants.Device_Auto_Push_D2S);
                await aa.Send(response);

            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }
        /// <summary>
        /// Kiểm tra user có trong dữ liệu chưa
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> CheckUser(Hanet_User user)
        {
            Hanet_Response_Array result = null;
            try
            {
                var options = new RestClientOptions(ServerHanet)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/person/getUserInfoByAliasID", Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("token", HanetParam.Token.access_token);
                request.AddParameter("aliasID", user.aliasID);
                RestResponse response = await client.ExecuteAsync(request);

                var strResult = response.Content;
                Console.WriteLine(strResult);

                if (string.IsNullOrEmpty(strResult))
                    return false;

                result = JsonConvert.DeserializeObject<Hanet_Response_Array>(strResult);

                if (result == null) return false;

                if (result.returnCode != Hanet_Response_Static.SUCCESSCode)
                    return false;

                if (result.data.Any())
                {
                    return true;
                }


                return false;
            }
            catch (Exception ex)
            {

                Logger.Error(ex.Message);
                return false;
            }

        }

        /// <summary>
        /// Đẩy thông tin user lên  với ảnh dạng file
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Hanet_Response> AddUserToHanet(Hanet_User user)
        {
            Hanet_Response result = null;
            try
            {

                var options = new RestClientOptions(ServerHanet)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/person/register", Method.Post);
                request.AddHeader("Authorization", "Bearer " + HanetParam.Token.access_token);
                request.AlwaysMultipartFormData = true;
                request.AddParameter("token", HanetParam.Token.access_token);
                request.AddParameter("name", user.name);
                request.AddParameter("aliasID", user.aliasID);
                request.AddParameter("placeID", user.placeID);
                request.AddParameter("title", "Học sinh");
                request.AddParameter("type", "0");
                request.AddParameter("departmentID", "1");
                if (string.IsNullOrEmpty(user.base64file))
                    request.AddFile("file", "C:/Users/ADMIN/Desktop/2.jpg");
                else
                {
                    var faceBytes = ConvertToByteArray(user.base64file);
                    if (faceBytes != null)
                    {
                        request.AddFile("file", faceBytes, user.aliasID + ".jpg");
                    }
                }

                RestResponse response = await client.ExecuteAsync(request);

                var strResult = response.Content;
                Console.WriteLine(strResult);

                if (string.IsNullOrEmpty(strResult))
                    return null;

                result = JsonConvert.DeserializeObject<Hanet_Response>(strResult);

                return result;
            }
            catch (Exception ex)
            {

                Logger.Error(ex.Message);
                return null;
            }

        }

        /// <summary>
        /// Đẩy thông tin user lên  với ảnh dạng url
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Hanet_Response> AddUserToHanetUrl(Hanet_User user)
        {
            Hanet_Response result = null;
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://partner.hanet.ai/person/registerByUrl");

                var content = new MultipartFormDataContent
                {
                    { new StringContent(HanetParam.Token.access_token), "token" },
                    { new StringContent(user.name), "name" },
                    { new StringContent(user.Url), "url"  },
                    { new StringContent(user.aliasID), "aliasID" },
                    { new StringContent(user.placeID), "placeID" },
                    { new StringContent("H"), "title" }
                };
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var strResult = await response.Content.ReadAsStringAsync();
                Console.WriteLine(strResult);

                if (string.IsNullOrEmpty(strResult))
                    return null;

                result = JsonConvert.DeserializeObject<Hanet_Response>(strResult);

                return result;
            }
            catch (Exception ex)
            {

                Logger.Error(ex.Message);
                return null;
            }

        }

        /// <summary>
        /// Update thông tin user lên  với ảnh dạng file
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Hanet_Response> UpdateFaceToHanet(Hanet_User user)
        {
            Hanet_Response result = null;
            try
            {
                var options = new RestClientOptions(ServerHanet)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/person/updateByFaceImage", Method.Post);
                request.AlwaysMultipartFormData = true;
                request.AddParameter("token", HanetParam.Token.access_token);
                request.AddParameter("aliasID", user.aliasID);
                request.AddParameter("placeID", user.placeID);

                if (string.IsNullOrEmpty(user.base64file))
                    request.AddFile("file[]", "C:/Users/ADMIN/Desktop/2.jpg");
                else
                {
                    var faceBytes = ConvertToByteArray(user.base64file);
                    if (faceBytes != null)
                    {
                        request.AddFile("file[]", faceBytes, user.aliasID + ".jpg");
                    }
                }


                RestResponse response = await client.ExecuteAsync(request);
                Console.WriteLine(response.Content);

                if (string.IsNullOrEmpty(response.Content))
                    return null;

                result = JsonConvert.DeserializeObject<Hanet_Response>(response.Content);

                return result;
            }
            catch (Exception ex)
            {

                Logger.Error(ex.Message);
                return null;
            }

        }
        /// <summary>
        /// Xoá user có trong dữ liệu chưa
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> RemoveUser(Hanet_User user)
        {
            Hanet_Response result = null;
            try
            {
                var options = new RestClientOptions(ServerHanet)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/person/remove", Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("token", HanetParam.Token.access_token);
                request.AddParameter("aliasID", user.aliasID);
                RestResponse response = await client.ExecuteAsync(request);

                var strResult = response.Content;
                Console.WriteLine(strResult);

                if (string.IsNullOrEmpty(strResult))
                    return false;

                result = JsonConvert.DeserializeObject<Hanet_Response>(strResult);

                if (result == null) return false;

                if (result.returnCode != Hanet_Response_Static.SUCCESSCode)
                    return false;

                return true;
            }
            catch (Exception ex)
            {

                Logger.Error(ex.Message);
                return false;
            }

        }

        /// <summary>
        /// Chuyển ảnh thành stream content
        /// </summary>
        /// <param name="base64encodedstring"></param>
        /// <returns></returns>
        public byte[] ConvertToByteArray(string base64Image) { return Convert.FromBase64String(base64Image); }
        #endregion
        /// <summary>
        /// Lưu thông tin lệnh
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task AddCommand(hanet_commandlog command)
        {
            try
            {
                bool add = false;

                var data = _deviceAutoPushDbContext.hanet_commandlog.FirstOrDefault(x => x.Id == command.Id);
                if (data == null)
                {
                    add = true;
                    data = new hanet_commandlog();
                    data.Id = command.Id;
                    data.create_time = DateTime.Now;
                }
                data.transfer_time = DateTime.Now;
                data.terminal_id = command.terminal_id;
                data.terminal_sn = command.terminal_sn;
                data.content = command.content;
                data.command_type = command.command_type;
                data.command_ation = command.command_ation;
                data.successed = command.successed;
                data.commit_time = command.commit_time;
                data.return_content = command.return_content;
                data.return_value = command.return_value;
                data.return_time = command.return_time;
                data.successed = command.successed;

                if (add)
                {
                    _deviceAutoPushDbContext.hanet_commandlog.Add(data);
                }
                _deviceAutoPushDbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
                return;
            }
        }
        /// <summary>
        /// Lưu thông tin thiết bị
        /// </summary>
        /// <param name="tA_Device"></param>
        /// <returns></returns>
        public async Task<bool> SaveDevice(TA_Device tA_Device)
        {
            try
            {
                bool add = false;
                var data = _deviceAutoPushDbContext.hanet_terminal.FirstOrDefault(x => x.Id == tA_Device.Id);
                if (data == null)
                {
                    add = true;
                    data = new hanet_terminal();
                    data.create_time  = DateTime.Now;
                    data.Id = tA_Device.Id;
                }
                data.Id = tA_Device.Id;
                data.sn = tA_Device.SerialNumber;
                data.name = tA_Device.DeviceName;
                if (add)
                {
                    await _deviceAutoPushDbContext.hanet_terminal.AddAsync(data);
                }
                await _deviceAutoPushDbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Lưu thông tin thiết bị
        /// </summary>
        /// <param name="tA_Device"></param>
        /// <returns></returns>
        public async Task<bool> RemoveDevice(TA_Device tA_Device)
        {
            try
            {
                var data = _deviceAutoPushDbContext.hanet_terminal.FirstOrDefault(x => x.Id == tA_Device.Id);
                if (data == null)
                {
                    return true;
                }
                _deviceAutoPushDbContext.hanet_terminal.Remove(data);
                await _deviceAutoPushDbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
                return false;
            }
        }

    }
    public class Hanet_Response
    {
        public int? statusCode { get; set; }
        public int returnCode { get; set; }
        public string returnMessage { get; set; }
        public Object data { get; set; }
    }
    public class Hanet_Response_Array
    {
        public int? statusCode { get; set; }
        public int returnCode { get; set; }
        public string returnMessage { get; set; }
        public List<Object> data { get; set; }
    }

    public class Hanet_Response_Static
    {
        public static int SUCCESSCode = 1;
        public static string SUCCESS = "SUCCESS";

    }

    public class Hanet_User
    {
        /// <summary>
        /// Tên người dùng
        /// </summary>
        public string? name { get; set; }
        /// <summary>
        /// Mã người dùng
        /// </summary>
        public string? aliasID { get; set; }
        /// <summary>
        /// Vị trí
        /// </summary>
        public string? placeID { get; set; }
        /// <summary>
        /// Chức danh
        /// </summary>
        public string? title { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public string? sex { get; set; }
        /// <summary>
        /// tuổi
        /// </summary>
        public string? age { get; set; }
        /// <summary>
        /// ngày sinh
        /// </summary>
        public string? dob { get; set; }
        /// <summary>
        /// file ảnh
        /// </summary>
        public string? base64file { get; set; }
        /// <summary>
        /// url ảnh
        /// </summary>
        public string? Url { get; set; }

    }
   
}
