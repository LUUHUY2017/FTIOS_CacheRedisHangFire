using AMMS.Hanet.Data;
using AMMS.Hanet.Data.Response;
using Newtonsoft.Json;
using RestSharp;
using Shared.Core.Loggers;

namespace AMMS.Hanet.Applications.V1.Service
{
    public class HANET_API_Service
    {
        public static string ServerHanet = "https://partner.hanet.ai";

        public HANET_API_Service()
        {

        }

        /// <summary>
        /// Lấy thông tin user
        /// </summary>
        /// <param name="user_aliasID"></param>
        /// <returns></returns>
        public async Task<Hanet_User> GetUser(string user_aliasID)
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
                request.AddParameter("aliasID", user_aliasID);
                RestResponse response = await client.ExecuteAsync(request);

                var strResult = response.Content;
                Console.WriteLine(strResult);

                if (string.IsNullOrEmpty(strResult))
                    return null;

                result = JsonConvert.DeserializeObject<Hanet_Response_Array>(strResult);

                if (result == null) return null;

                if (result.returnCode != Hanet_Response_Static.SUCCESSCode)
                    return null;

                if (result.data.Any())
                {
                    Hanet_User user_return = (Hanet_User)result.data.FirstOrDefault();

                    return user_return;
                }


                return null;
            }
            catch (Exception ex)
            {

                Logger.Error(ex);
                return null;
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

                Logger.Error(ex);
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
                request.AddParameter("departmentID", user.departmentID);
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

                Logger.Error(ex);
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

                Logger.Error(ex);
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

                Logger.Error(ex);
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

                Logger.Error(ex);
                return false;
            }

        }

        /// <summary>
        /// Lấy tổng số user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<int> GetCountUserByPlace(string placeId)
        {
            Hanet_Response result = null;
            try
            {
                var options = new RestClientOptions(ServerHanet)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/person/getTotalPersonByPlaceID", Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("token", HanetParam.Token.access_token);
                request.AddParameter("placeID", placeId);
                request.AddParameter("type", "0");
                RestResponse response = await client.ExecuteAsync(request);

                var strResult = response.Content;
                Console.WriteLine(strResult);

                if (string.IsNullOrEmpty(strResult))
                    return 0;

                result = JsonConvert.DeserializeObject<Hanet_Response>(strResult);

                if (result == null) return 0;

                if (result.returnCode != Hanet_Response_Static.SUCCESSCode || result.data == null)
                    return 0;

                int countData = 0;

                countData = int.Parse(result.data.ToString());

                return countData;
            }
            catch (Exception ex)
            {

                Logger.Error(ex);
                return 0;
            }

        }

        /// <summary>
        /// Lấy user theo placeId
        /// </summary>
        /// <param name="placeId"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public async Task<List<Hanet_User_Data>> UserByPlaceId(string placeId, int page, int pagesize = 50)
        {
            try
            {
                List<Hanet_User_Data> listData = new List<Hanet_User_Data>();
                Hanet_Response_Array result = null;

                var options = new RestClientOptions(ServerHanet)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/person/getListByPlace", Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("token", HanetParamTest.Token.access_token);
                request.AddParameter("placeID", HanetParamTest.PlaceId);
                request.AddParameter("type", "0");
                request.AddParameter("page", page);
                request.AddParameter("size", pagesize);
                RestResponse response = await client.ExecuteAsync(request);

                var strResult = response.Content;
                Console.WriteLine(strResult);

                if (string.IsNullOrEmpty(strResult))
                    return listData;

                result = JsonConvert.DeserializeObject<Hanet_Response_Array>(strResult);

                if (result == null) return listData;

                if (result.returnCode != Hanet_Response_Static.SUCCESSCode)
                    return listData;
                foreach (var obj in result.data)
                {
                    string json = JsonConvert.SerializeObject(obj);

                    var h_user = JsonConvert.DeserializeObject<Hanet_User_Data>(json);
                    if (h_user != null)
                        listData.Add(h_user);
                }

                return listData;
            }
            catch (Exception ex)
            {

                Logger.Error(ex);
                return new List<Hanet_User_Data>();
            }

        }

        /// <summary>
        /// Chuyển ảnh thành stream content
        /// </summary>
        /// <param name="base64encodedstring"></param>
        /// <returns></returns>
        public byte[] ConvertToByteArray(string base64Image) { return Convert.FromBase64String(base64Image); }
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
        public string? departmentID { get; set; }
    }
    public class HanetParamTest
    {
        public static string Host { get; set; } = "https://oauth.hanet.com/";
        public static string PlaceId { get; set; } = "16588";
        public static long TimeExpireTick { get; set; }
        public DateTime TimeExpires
        {
            get
            {
                return new DateTime(TimeExpireTick);
            }
        }
        public static AccessToken Token { get; set; } = new AccessToken()
        {
            refresh_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjUwMjQxOTY1NTE2ODA2ODE2NDMiLCJlbWFpbCI6InRyYW5raHVlaHV0QGdtYWlsLmNvbSIsImNsaWVudF9pZCI6IjljOGNmYmQyNTc2ODlhMmE1NDhlNTZlYWI1Njg4NTg1IiwidHlwZSI6ImF1dGhvcml6YXRpb25fY29kZSIsImlhdCI6MTczMTExODQ2MywiZXhwIjoxNzYyNjU0NDYzfQ.lzORDkrCWj95VujvKo03nCNwghbUYleMBGklyuYSLY8",
            access_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjUwMjQxOTY1NTE2ODA2ODE2NDMiLCJlbWFpbCI6InRyYW5raHVlaHV0QGdtYWlsLmNvbSIsImNsaWVudF9pZCI6IjljOGNmYmQyNTc2ODlhMmE1NDhlNTZlYWI1Njg4NTg1IiwidHlwZSI6InJlZnJlc2hfdG9rZW4iLCJpYXQiOjE3MzExMTg0NjMsImV4cCI6MTc2MjY1NDQ2M30.iHnLrTWPaXBGdP9ndq0n9Gk1MC_JeYbduVrT2Xa0BcA",
            expire = 1761709129,
            token_type = "bearer"
        };

    }
}
