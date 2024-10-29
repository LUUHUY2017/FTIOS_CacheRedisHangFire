using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Application.Services.VTSmart.Responses;
using Server.Core.Entities.A0;
using Server.Core.Identity.Entities;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Loggers;
using System.Security.Cryptography;
using System.Text;

namespace Server.Application.Services.VTSmart;

public sealed class SmartService
{
    private readonly UserManager<ApplicationUser> _userMgr;
    private readonly MasterDataDbContext _dbContext;
    public string UserId { get; set; }

    public SmartService(
        UserManager<ApplicationUser> userMgr,
        MasterDataDbContext dbContext
        )
    {
        _userMgr = userMgr;
        _dbContext = dbContext;
    }

    public static string urlServerName = "https://gateway.vtsmas.vn";
    public static string urlSSO = "https://sso.vtsmas.vn/connect/token";
    public static AccessToken _accessToken;

    public async Task<A0_AttendanceConfig> GetConfig()
    {
        A0_AttendanceConfig retval = null;
        try
        {
            retval = await _dbContext.A0_AttendanceConfig.Where(o => o.Actived == true).FirstOrDefaultAsync();
            if (retval != null)
            {
                urlSSO = retval.EndpointIdentity;
                urlServerName = retval.EndpointGateway;
            }
            else
            {
                retval = new A0_AttendanceConfig()
                {
                    EndpointIdentity = "https://sso.vtsmas.vn/connect/token",
                    AccountName = "lsn_thcs_yenvuong",
                    Password = "Vucuong@1971",
                };
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return retval;
    }

    public async Task<AccessToken> GetToken()
    {
        AccessToken retval = null;
        try
        {
            var conf = await GetConfig();
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, urlSSO);
            var content = new MultipartFormDataContent();

            content.Add(new StringContent("password"), "grant_type");
            content.Add(new StringContent("openid profile IdentityService TenantService InternalGateway BackendAdminAppGateway EmployeeService CategoryService SmasCustomerService AdminSettingService SettingService ClassroomSupervisorService StudentService ScoreBookService MongoDynamicPageService"), "scope");
            content.Add(new StringContent(conf.AccountName), "username");
            content.Add(new StringContent(conf.Password), "password");
            content.Add(new StringContent("backend-admin-app-client"), "client_id");
            content.Add(new StringContent("1q2w3e*"), "client_secret");

            request.Content = content;
            var result = await client.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var data = await result.Content.ReadAsStringAsync();
                retval = JsonConvert.DeserializeObject<AccessToken>(data);
                _accessToken = new AccessToken(retval.access_token, retval.expires_in, retval.token_type, retval.scope);
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }

        return retval;
    }

    #region GET
    public async Task<CurrentUserInfo> PostCurrentUser()
    {
        CurrentUserInfo retval = new CurrentUserInfo();
        try
        {
            if (_accessToken == null || !_accessToken.IsTokenValid())
                await GetToken();
            if (_accessToken != null)
            {
                var api = string.Format("{0}/api/permission-management/permissions/permission-granted", urlServerName);
                var parameter = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken.access_token);

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
    public async Task<List<SchoolYearResponse>> PostSchoolYears()
    {
        List<SchoolYearResponse> retval = null;
        try
        {
            if (_accessToken == null || !_accessToken.IsTokenValid())
                await GetToken();

            if (_accessToken != null)
            {
                var api = string.Format("{0}/api/danh-muc-truong/nam-hoc-nha-truong/tat-ca", urlServerName);
                var parameter = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken.access_token);

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

    public async Task<SchoolResponse> PostSchool()
    {
        SchoolResponse retval = null;
        try
        {
            if (_accessToken == null || !_accessToken.IsTokenValid())
                await GetToken();
            if (_accessToken != null)
            {
                var api = string.Format("{0}/api/truong-hoc/public/current", urlServerName);
                var parameter = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken.access_token);

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

    public async Task<List<StudentResponse>> PostStudents(string classId, string schoolYearId)
    {
        List<StudentResponse> retval = new List<StudentResponse>();
        try
        {
            if (_accessToken == null || !_accessToken.IsTokenValid())
                await GetToken();

            if (_accessToken != null)
            {
                var api = string.Format("{0}/api/hoc-sinh/lay-hoc-sinh-theo-lop/{1}/{2}", urlServerName, classId, schoolYearId);
                var parameter = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken.access_token);
                    var result = await client.GetAsync(api);
                    if (result.IsSuccessStatusCode)
                    {
                        var data = await result.Content.ReadAsStringAsync();
                        retval = JsonConvert.DeserializeObject<List<StudentResponse>>(data);
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

    public async Task<List<GradeClasseReponse>> PostClass(string schoolLevelCode, string schoolYearId, string schoolyear)
    {
        List<GradeClasseReponse> retval = new List<GradeClasseReponse>();
        try
        {
            if (_accessToken == null || !_accessToken.IsTokenValid())
                await GetToken();
            if (_accessToken != null)
            {
                var api = string.Format("{0}/api/hoc-tap/phan-cong-giao-vu/lay-khoi-lop-theo-user-login/{1}/{2}?viewTeaching=true", urlServerName, schoolLevelCode, schoolYearId);
                var parameter = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken.access_token);
                    client.DefaultRequestHeaders.Add("Schoolyear", schoolyear);

                    var result = await client.GetAsync(api);
                    if (result.IsSuccessStatusCode)
                    {
                        var data = await result.Content.ReadAsStringAsync();
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

    #region POST
    public async Task<List<GradeClasseReponse>> PostSyncAttendence2VSMAS()
    {
        List<GradeClasseReponse> retval = null;
        try
        {
            if (_accessToken == null || !_accessToken.IsTokenValid())
                await GetToken();
            if (_accessToken != null)
            {
                string secretKey = "";

                var studentAbsenceByDevices = new List<StudentAbsenceByDevice>()
                {
                    new StudentAbsenceByDevice
                    {
                      StudentCode =  "HS0001",
                      Value= "X",
                      ExtraProperties= new ExtraProperties()
                      {
                            IsLate= true,
                            IsOffSoon= true,
                            IsOffPeriod= true,
                            LateTime= DateTime.UtcNow,
                            OffSoonTime=DateTime.UtcNow,
                            PeriodI= false,
                            PeriodII= true,
                            PeriodIII= true,
                            PeriodIV= false,
                            PeriodV= false,
                            PeriodVI= false,
                            AbsenceTime= DateTime.UtcNow,
                      }
                    }
                };
                var paramData = new SyncDataResponse()
                {
                    SecretKey = secretKey,
                    SchoolCode = "7900001",
                    SchoolYearCode = "2023-2024",
                    ClassCode = "LH0001",
                    AbsenceDate = DateTime.UtcNow,
                    Section = 0,
                    FormSendSMS = 1,
                    StudentAbsenceByDevices = studentAbsenceByDevices,
                };


                var api = string.Format("{0}/api/hoc-tap/diem-danh-hoc-sinh/diem-danh-tich-hop-thiet-bi");
                var parameter = new StringContent(JsonConvert.SerializeObject(paramData), Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken.access_token);

                    var result = await client.PostAsync(api, parameter);
                    if (result.IsSuccessStatusCode)
                    {
                        var data = await result.Content.ReadAsStringAsync();
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
    public static string GetSecretKeyVMSAS(string secretKey, string keyHas, string ivHas, string schoolCode)
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
        byte[] iv = Convert.FromBase64String(ivHas);
        string encryptedString = EncryptStringVSMAS(plaintext, key, iv);

        return encryptedString;
    }
    #endregion





}
