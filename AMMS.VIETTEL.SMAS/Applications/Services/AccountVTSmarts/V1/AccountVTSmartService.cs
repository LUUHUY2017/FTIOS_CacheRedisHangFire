using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart.Responses;
using AMMS.VIETTEL.SMAS.Cores.Entities.A0;
using AMMS.VIETTEL.SMAS.Cores.Identity.Entities;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Application.Services.VTSmart;
using Shared.Core.Loggers;
using System.Text;

namespace AMMS.VIETTEL.SMAS.Applications.Services.AccountVTSmarts.V1;

public sealed class AccountVTSmartService
{
    private readonly UserManager<ApplicationUser> _userMgr;
    private readonly IViettelDbContext _dbContext;
    private readonly SmartService _smartService;
    public string UserId { get; set; }

    public AccountVTSmartService(
        UserManager<ApplicationUser> userMgr,
        IViettelDbContext dbContext,
        SmartService smartService
        )
    {
        _userMgr = userMgr;
        _dbContext = dbContext;
        _smartService = smartService;
    }

    public static string urlServerName = "https://gateway.vtsmas.vn";
    public static string urlSSO = "https://sso.vtsmas.vn/connect/token";
    public static AccessToken _accessToken;

    public async Task<A0_AttendanceConfig> GetConfig()
    {
        A0_AttendanceConfig retval = null;
        try
        {
            retval = await _dbContext.app_config.Where(o => o.Actived == true).FirstOrDefaultAsync();
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

    public async Task<AccessToken> GetToken(string accountName, string password)
    {
        AccessToken retval = null;
        try
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, urlSSO);
            var content = new MultipartFormDataContent();

            content.Add(new StringContent("password"), "grant_type");
            content.Add(new StringContent("openid profile IdentityService TenantService InternalGateway BackendAdminAppGateway EmployeeService CategoryService SmasCustomerService AdminSettingService SettingService ClassroomSupervisorService StudentService ScoreBookService MongoDynamicPageService"), "scope");
            content.Add(new StringContent(accountName), "username");
            content.Add(new StringContent(password), "password");
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
    public async Task<CurrentUserInfo> PostUserVT(string accountName, string password)
    {
        CurrentUserInfo retval = new CurrentUserInfo();
        try
        {
            if (_accessToken == null || !_accessToken.IsTokenValid())
                await GetToken(accountName, password);
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
    #endregion
}
