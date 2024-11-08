using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart.Responses;
using AMMS.VIETTEL.SMAS.Cores.Entities.A0;
using AMMS.VIETTEL.SMAS.Cores.Identity.Entities;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using Shared.Core.Loggers;
using System.Security.Cryptography;
using System.Text;

namespace AMMS.VIETTEL.SMAS.Applications.Services.VTSmart;

public sealed class SmartService
{
    private readonly UserManager<ApplicationUser> _userMgr;
    private readonly ViettelDbContext _dbContext;
    public string UserId { get; set; }

    public SmartService(
        UserManager<ApplicationUser> userMgr,
        ViettelDbContext dbContext
        )
    {
        _userMgr = userMgr;
        _dbContext = dbContext;
    }

    public static string urlServerName = "https://gateway.vtsmas.vn";
    public static string urlSSO = "https://sso.vtsmas.vn/connect/token";
    public static AccessToken _accessToken;

    public static string key { get; set; } = "r0QQKLBa3x9KN/8el8Q/HQ==";
    public static string keyIV = "8bCNmt1+RHBNkXRx8MlKDA==";
    public static string secretKey = "SMas$#3/*/lsn_diem_danh";


    public async Task<AccessToken> GetToken1(string orgId)
    {
        AccessToken accessToken = null;
        try
        {
            AttendanceConfig retval = await _dbContext.AttendanceConfig.Where(o => o.Actived == true && o.OrganizationId == orgId).FirstOrDefaultAsync();
            if (retval != null)
            {
                if (string.IsNullOrWhiteSpace(retval.access_token) || retval.time_expires_in.Value <= DateTime.Now)
                {
                    var token = await RefreshToken(retval);
                    if (token != null)
                    {
                        retval.expires_in = token.expires_in;
                        retval.access_token = token.access_token;
                        retval.time_expires_in = DateTime.Now.AddSeconds(token.expires_in);
                        retval.Scope = token.scope;
                        retval.token_type = token.token_type;
                        await _dbContext.SaveChangesAsync();
                    }
                }

                accessToken = new AccessToken()
                {
                    endpoint_gateway = retval.EndpointGateway,
                    endpoint_identity = retval.EndpointIdentity,

                    access_token = retval.access_token,
                    scope = retval.Scope,
                    token_type = retval.token_type,
                    expires_in = retval.expires_in.Value,
                    time_expires_in = retval.time_expires_in.Value,
                };
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return accessToken;
    }
    public async Task<AccessToken> RefreshToken(AttendanceConfig conf)
    {
        AccessToken retval = null;
        try
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, conf.EndpointIdentity);
            var content = new MultipartFormDataContent();

            content.Add(new StringContent(conf.GrantType), "grant_type");
            content.Add(new StringContent(conf.Scope), "scope");
            content.Add(new StringContent(conf.AccountName), "username");
            content.Add(new StringContent(conf.Password), "password");
            content.Add(new StringContent(conf.ClientId), "client_id");
            content.Add(new StringContent(conf.ClientSecret), "client_secret");

            request.Content = content;
            var result = await client.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var data = await result.Content.ReadAsStringAsync();
                retval = JsonConvert.DeserializeObject<AccessToken>(data);
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return retval;
    }

    //DEV TEST khi chưa có V2
    public async Task<AccessToken> GetToken(string id)
    {
        AccessToken accessToken = null;
        try
        {
            string urlSSO = "https://sso.vtsmas.vn/connect/token";
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, urlSSO);
            var content = new MultipartFormDataContent();

            content.Add(new StringContent("password"), "grant_type");
            content.Add(new StringContent("openid profile IdentityService TenantService InternalGateway BackendAdminAppGateway EmployeeService CategoryService SmasCustomerService AdminSettingService SettingService ClassroomSupervisorService StudentService ScoreBookService MongoDynamicPageService"), "scope");
            content.Add(new StringContent("lsn_thcs_yenvuong"), "username");
            content.Add(new StringContent("Vucuong@1971"), "password");
            content.Add(new StringContent("backend-admin-app-client"), "client_id");
            content.Add(new StringContent("1q2w3e*"), "client_secret");


            request.Content = content;
            var result = await client.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var data = await result.Content.ReadAsStringAsync();
                accessToken = JsonConvert.DeserializeObject<AccessToken>(data);
                accessToken.endpoint_gateway = urlServerName;
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return accessToken;
    }


    #region GET V1
    public async Task<CurrentUserInfo> PostCurrentUser(string orgId)
    {
        CurrentUserInfo retval = new CurrentUserInfo();
        try
        {
            var accessToken = await GetToken(orgId);
            if (accessToken != null)
            {
                var api = string.Format("{0}/api/permission-management/permissions/permission-granted", accessToken.endpoint_gateway);
                var parameter = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken.access_token);

                    var result = await client.GetAsync(api);
                    if (result.IsSuccessStatusCode)
                    {
                        var data = await result.Content.ReadAsStringAsync();
                        retval = JsonConvert.DeserializeObject<CurrentUserInfo>(data);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return retval;
    }
    public async Task<List<SchoolYearResponse>> PostSchoolYears(string orgId)
    {
        List<SchoolYearResponse> retval = null;
        try
        {
            var accessToken = await GetToken(orgId);

            if (accessToken != null)
            {
                var api = string.Format("{0}/api/danh-muc-truong/nam-hoc-nha-truong/tat-ca", accessToken.endpoint_gateway);
                var parameter = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken.access_token);

                    var result = await client.GetAsync(api);
                    if (result.IsSuccessStatusCode)
                    {
                        var data = await result.Content.ReadAsStringAsync();
                        retval = JsonConvert.DeserializeObject<List<SchoolYearResponse>>(data);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return retval;
    }

    public async Task<SchoolResponse> PostSchool(string orgId)
    {
        SchoolResponse retval = null;
        try
        {
            var accessToken = await GetToken(orgId);
            if (accessToken != null)
            {
                var api = string.Format("{0}/api/truong-hoc/public/current", accessToken.endpoint_gateway);
                var parameter = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken.access_token);

                    var result = await client.GetAsync(api);
                    if (result.IsSuccessStatusCode)
                    {
                        var data = await result.Content.ReadAsStringAsync();
                        retval = JsonConvert.DeserializeObject<SchoolResponse>(data);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return retval;
    }

    public async Task<List<StudentSmasResponse>> PostStudents(string classId, string schoolYearId, string orgId)
    {
        List<StudentSmasResponse> retval = new List<StudentSmasResponse>();
        try
        {
            var accessToken = await GetToken(orgId);

            if (accessToken != null)
            {
                var api = string.Format("{0}/api/hoc-sinh/lay-hoc-sinh-theo-lop/{1}/{2}", accessToken.endpoint_gateway, classId, schoolYearId);
                var parameter = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken.access_token);
                    var result = await client.GetAsync(api);
                    if (result.IsSuccessStatusCode)
                    {
                        var data = await result.Content.ReadAsStringAsync();
                        retval = JsonConvert.DeserializeObject<List<StudentSmasResponse>>(data);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return retval;
    }

    public async Task<List<GradeClasseReponse>> PostClass(string schoolLevelCode, string schoolYearId, string schoolyear, string orgId)
    {
        List<GradeClasseReponse> retval = new List<GradeClasseReponse>();
        try
        {
            var accessToken = await GetToken(orgId);
            if (accessToken != null)
            {
                var api = string.Format("{0}/api/hoc-tap/phan-cong-giao-vu/lay-khoi-lop-theo-user-login/{1}/{2}?viewTeaching=true", accessToken.endpoint_gateway, schoolLevelCode, schoolYearId);
                var parameter = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken.access_token);
                    client.DefaultRequestHeaders.Add("Schoolyear", schoolyear);

                    var result = await client.GetAsync(api);
                    if (result.IsSuccessStatusCode)
                    {
                        var data = await result.Content.ReadAsStringAsync();
                        if (!string.IsNullOrWhiteSpace(data))
                            retval = await ConvertDicToGradeClasseReponse(JsonConvert.DeserializeObject<Dictionary<string, List<ClassResponse>>>(data));
                    }
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return retval;
    }

    public async Task<List<GradeClasseReponse>> ConvertDicToGradeClasseReponse(Dictionary<string, List<ClassResponse>> dictionary)
    {
        var result = new List<GradeClasseReponse>();

        foreach (var kvp in dictionary)
        {
            result.Add(new GradeClasseReponse
            {
                GradeLevelCode = kvp.Key,
                Classes = kvp.Value
            });
        }
        return result;
    }
    #endregion





    #region POST V2
    public async Task<List<StudenSmasApiResponse>> PostListStudents(string provinceCode, string schoolCode, string schoolYearCode)
    {
        List<StudenSmasApiResponse> retval = new List<StudenSmasApiResponse>();
        try
        {
            //var accessToken = await GetToken(orgId);

            //if (accessToken != null)
            {
                string _secretKey = GetSecretKeySMAS(secretKey, key, keyIV, schoolCode); //  "20186511"
                var req = new StudentSmasApiRequest()
                {
                    secretKey = _secretKey,
                    provinceCode = provinceCode,
                    schoolCode = schoolCode,
                    schoolYearCode = schoolYearCode,
                };

                var api = string.Format("{0}/api/hoc-tap/diem-danh-hoc-sinh/lay-danh-sach-hoc-sinh-diem-danh-thiet-bi", urlServerName);
                var parameter = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken.access_token);

                    var result = await client.PostAsync(api, parameter);
                    if (result.IsSuccessStatusCode)
                    {
                        var data = await result.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<StudentDataApiResponse>(data);
                        if (res.IsSuccess)
                        {
                            retval = res.Responses;
                        }
                        else
                        {
                            throw new InvalidOperationException(res.Message);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            throw new Exception("Error occurred while fetching student data.", ex);
        }
        return retval;
    }
    public async Task<SyncDataResponse> PostSyncAttendence2Smas(SyncDataRequest req, string schoolCode)
    {
        SyncDataResponse retval = null;
        try
        {
            //var _accessToken = await GetToken(orgId);
            //if (_accessToken != null)
            {
                string _secretKey = GetSecretKeySMAS(secretKey, key, keyIV, schoolCode); //"20186511"
                req.SecretKey = _secretKey;
                var api = string.Format("{0}/api/hoc-tap/diem-danh-hoc-sinh/diem-danh-tich-hop-thiet-bi", urlServerName);
                var parameter = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken.access_token);

                    var result = await client.PostAsync(api, parameter);
                    if (result.IsSuccessStatusCode)
                    {
                        var data = await result.Content.ReadAsStringAsync();
                        retval = JsonConvert.DeserializeObject<SyncDataResponse>(data);

                    }
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return retval;
    }


    public string EncryptStringSMAS(string plaintext, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            {
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
                byte[] ciphertextBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);
                return Convert.ToBase64String(ciphertextBytes);
            }
        }
    }
    public string GetSecretKeySMAS(string secretKey, string keyHas, string keyIV, string schoolCode)
    {
        TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // UTC+7 (Indochina Time)
        DateTime currentDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timezone);

        string dateTimeFormatted = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        // $plaintext cần phải đúng định dạng <SecretKey>||<Mã trường CSDL>||<Ngày hiện tại> ví dụ: “Abc@123||TH00001||2024-07-30 HH:mm:ss”
        // Ngày hiện tại được tính là ngày của thời điểm call api, định dạng yyyy-MM-dd HH:mm:ss
        // Bắt buộc truyền đúng plaintext sau đó encrypt plaintext để có kết quả đầu ra
        // SecretKey sẽ được cấp thông qua đầu mối tích hợp

        string plaintext = $"{secretKey}||{schoolCode}||{dateTimeFormatted}";

        byte[] key = Convert.FromBase64String(keyHas);
        byte[] iv = Convert.FromBase64String(keyIV);
        string encryptedString = EncryptStringSMAS(plaintext, key, iv);

        return encryptedString;
    }
    #endregion

    public static async Task<StudentResponse1> GetStudents(string schoolCode)
    {
        string _secretKey = GetSecretKeyVMSAS("SMas$#@$20@4/*/lsn-diem-danh-app-client", key, keyIV, schoolCode);

        var api = string.Format("https://gateway.vtsmas.vn/api/hoc-tap/diem-danh-hoc-sinh/lay-danh-sach-hoc-sinh-diem-danh-thiet-bi");
        var parameter = new StringContent(JsonConvert.SerializeObject(new
        {
            secretKey = _secretKey,
            schoolCode

        }), Encoding.UTF8, "application/json");
        using (HttpClient client = new HttpClient())
        {
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken.access_token);

            var result = await client.PostAsync(api, parameter);
            if (result.IsSuccessStatusCode)
            {
                var data = await result.Content.ReadAsStringAsync();
                var retval = JsonConvert.DeserializeObject<StudentResponse1>(data);
                return retval;
            }
            else
            {
                return new StudentResponse1()
                {
                    isSuccess = false,
                    message = await result.Content.ReadAsStringAsync(),
                };
            }
        }
    }
    public static string EncryptStringVSMAS(string plaintext, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            {
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
                byte[] ciphertextBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);
                return Convert.ToBase64String(ciphertextBytes);
            }
        }
    }
    public static string GetSecretKeyVMSAS(string secretKey, string keyHas, string keyIV, string schoolCode)
    {
        TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // UTC+7 (Indochina Time)
        DateTime currentDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timezone);

        string dateTimeFormatted = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        // $plaintext cần phải đúng định dạng <SecretKey>||<Mã trường CSDL>||<Ngày hiện tại> ví dụ: “Abc@123||TH00001||2024-07-30 HH:mm:ss”
        // Ngày hiện tại được tính là ngày của thời điểm call api, định dạng yyyy-MM-dd HH:mm:ss
        // Bắt buộc truyền đúng plaintext sau đó encrypt plaintext để có kết quả đầu ra
        // SecretKey sẽ được cấp thông qua đầu mối tích hợp

        string plaintext = $"{secretKey}||{schoolCode}||{dateTimeFormatted}";

        byte[] key = Convert.FromBase64String(keyHas);
        byte[] iv = Convert.FromBase64String(keyIV);
        string encryptedString = EncryptStringVSMAS(plaintext, key, iv);

        return encryptedString;
    }
}
