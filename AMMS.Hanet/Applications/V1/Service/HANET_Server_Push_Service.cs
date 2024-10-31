using AMMS.DeviceData.RabbitMq;
using AMMS.Hanet.Data;
using AMMS.Hanet.Datas.Databases;
using AMMS.Hanet.Datas.Entities;
using Newtonsoft.Json;
using RestSharp;
using Shared.Core.Loggers;
using System.Text.Json.Serialization;

namespace AMMS.Hanet.Applications.V1.Service
{
    public class HANET_Server_Push_Service
    {
        DeviceAutoPushDbContext _deviceAutoPushDbContext;
        public HANET_Server_Push_Service(DeviceAutoPushDbContext deviceAutoPushDbContext)
        {
            _deviceAutoPushDbContext = deviceAutoPushDbContext;
        }

        #region Pust data user
        public async Task ProcessDataServerPush(RB_ServerRequest rB_ServerRequest)
        {
            try
            {
                Logger.Warning("Data :" + rB_ServerRequest.RequestParam);

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

                //wait AddCommand(conmandlog);

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
                        placeID = "28340",
                    };

                    if (await CheckUser(user))
                    {
                        var result = await UpdateFaceToHanet(user);
                        if (result != null)
                        {
                            conmandlog.return_time = DateTime.Now;
                            conmandlog.return_content = result.returnMessage;
                            conmandlog.return_value = result.returnCode;

                        }
                        else
                        {
                            conmandlog.return_time = DateTime.Now;
                            conmandlog.return_content = "Không kết nối được server hanet";
                            conmandlog.return_value = null;
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
                        }
                        else
                        {
                            conmandlog.return_time = DateTime.Now;
                            conmandlog.return_content = "Không kết nối được server hanet";
                            conmandlog.return_value = null;
                        }
                    }
                    await AddCommand(conmandlog);
                }
                if (rB_ServerRequest.Action == ServerRequestAction.ActionDelete && rB_ServerRequest.RequestType == ServerRequestType.UserInfo)
                {
                    TA_PersonInfo? data = JsonConvert.DeserializeObject<TA_PersonInfo>(rB_ServerRequest.RequestParam);

                    if (data == null)
                        return;
                    Hanet_User user = new Hanet_User()
                    {
                        base64file = data.UserFace,
                        name = data.FullName,
                        aliasID = data.PersonCode,
                        placeID = "28340",
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
                var options = new RestClientOptions("https://partner.hanet.ai")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/person/getUserInfoByAliasID", Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("token", HanetParamTest.Token.access_token);
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

                var options = new RestClientOptions("https://partner.hanet.ai")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/person/register", Method.Post);
                request.AddHeader("Authorization", "Bearer " + HanetParamTest.Token.access_token);
                request.AlwaysMultipartFormData = true;
                request.AddParameter("token", HanetParamTest.Token.access_token);
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
                var options = new RestClientOptions("https://partner.hanet.ai")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/person/updateByFaceImage", Method.Post);
                request.AlwaysMultipartFormData = true;
                request.AddParameter("token", HanetParamTest.Token.access_token);
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
                var options = new RestClientOptions("https://partner.hanet.ai")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/person/remove", Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("token", HanetParamTest.Token.access_token);
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
        public StreamContent? ConvertToStream(string base64encodedstring)
        {
            try
            {
                if (string.IsNullOrEmpty(base64encodedstring))
                    return null;
                var bytes = Convert.FromBase64String(base64encodedstring);
                var contents = new StreamContent(new MemoryStream(bytes));
                return contents;
            }
            catch (Exception ex)
            {

                Logger.Error(ex.Message);
                return null;
            }
        }
        public byte[] ConvertToByteArray(string base64Image) { return Convert.FromBase64String(base64Image); }
        #endregion

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
        public async Task UpdateCommand(hanet_commandlog command)
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
                }

                data.commit_time = command.commit_time;
                data.terminal_id = command.terminal_id;
                data.terminal_sn = command.terminal_sn;
                data.content = command.content;
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
    public class HanetParamTest
    {
        public static string Host { get; set; } = "https://oauth.hanet.com/";
        public static AccessToken Token { get; set; } = new AccessToken()
        {
            refresh_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjQxMjYyMTM0MDE3Mjc3NDY2NDEiLCJlbWFpbCI6Im5hbW5kQGFjcy52biIsImNsaWVudF9pZCI6ImUwZjM0NWRhNWYxODdkMjZiMDE4ZTFkMzYwM2FkOGE4IiwidHlwZSI6InJlZnJlc2hfdG9rZW4iLCJpYXQiOjE3MzAyNTU3MjEsImV4cCI6MTc5MzMyNzcyMX0.FIB2oRdQugm5rhLj_c8YskVPlgu7wTSkPmR7Xs6Zb-Y",
            access_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjQxMjYyMTM0MDE3Mjc3NDY2NDEiLCJlbWFpbCI6Im5hbW5kQGFjcy52biIsImNsaWVudF9pZCI6ImUwZjM0NWRhNWYxODdkMjZiMDE4ZTFkMzYwM2FkOGE4IiwidHlwZSI6InJlZnJlc2hfdG9rZW4iLCJpYXQiOjE3MzAyNTMzNTAsImV4cCI6MTc5MzMyNTM1MH0.KnzcNIQKBR8WcX9OwtVFCiRXzPnTvUPuJiSP4YGCZvg",
            expire = 1761791721,
            token_type = "bearer"
        };

    }
}
