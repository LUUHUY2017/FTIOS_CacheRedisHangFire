using AMMS.VIETTEL.SMAS.Applications.Services.AppConfigs.V1.Models;
using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart;
using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart.Responses;
using AMMS.VIETTEL.SMAS.Cores.Entities.A0;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.AppConfigs;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Organizations;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.TimeConfigs;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Core.Commons;
using Shared.Core.Loggers;
using System.Text;

namespace AMMS.VIETTEL.SMAS.Applications.Services.AppConfigs.V1;

public class AppConfigService
{
    private readonly IAppConfigRepository _appConfigRepository;
    private readonly IMapper _mapper;
    private readonly SmartService _smartService;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly ITimeConfigRepository _timeConfigRepository;
    private readonly IViettelDbContext _dBContext;
    public AppConfigService(
        IAppConfigRepository appConfigRepository, 
        IMapper mapper, 
        SmartService smartService,
        IOrganizationRepository organizationRepository,
        ITimeConfigRepository timeConfigRepository,
        IViettelDbContext dBContext
        )
    {
        _appConfigRepository = appConfigRepository;
        _mapper = mapper;
        _smartService = smartService;
        _organizationRepository = organizationRepository;
        _timeConfigRepository = timeConfigRepository;
        _dBContext = dBContext;
    }

    public static string urlServerName = "https://gateway.vtsmas.vn";
    public static string urlSSO = "https://sso.vtsmas.vn/connect/token";
    public static AccessTokenLocal _AccessTokenLocal;

    public static string key = "r0QQKLBa3x9KN/8el8Q/HQ==";
    public static string keyIV = "8bCNmt1+RHBNkXRx8MlKDA==";
    public static string secretKey = "Smas!@#2023";
    public async Task<AccessTokenLocal> GetToken(AttendanceConfig configModel)
    {
        AccessTokenLocal retval = null;
        try
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, urlSSO);
            var content = new MultipartFormDataContent();

            content.Add(new StringContent(configModel.GrantType), "grant_type");
            content.Add(new StringContent(configModel.Scope), "scope");
            content.Add(new StringContent(configModel.AccountName), "username");
            content.Add(new StringContent(configModel.Password), "password");
            content.Add(new StringContent(configModel.ClientId), "client_id");
            content.Add(new StringContent(configModel.ClientSecret), "client_secret");

            request.Content = content;
            var result = await client.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var data = await result.Content.ReadAsStringAsync();
                retval = JsonConvert.DeserializeObject<AccessTokenLocal>(data);
                _AccessTokenLocal = new AccessTokenLocal(retval.access_token, retval.expires_in, retval.token_type, retval.scope);
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }

        return retval;
    }

    public async Task<SchoolResponse> PostSchool(AttendanceConfig configModel)
    {
        SchoolResponse retval = null;
        try
        {
            if (_AccessTokenLocal == null || !_AccessTokenLocal.IsTokenValid())
                await GetToken(configModel);
            if (_AccessTokenLocal != null)
            {
                var api = string.Format("{0}/api/truong-hoc/public/current", urlServerName);
                var parameter = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _AccessTokenLocal.access_token);

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

    public async Task<Result<AttendanceConfig>> SchoolAsync(AttendanceConfig configModel)
    {
        try
        {
            // lấy trường theo config
            var postSchool = await PostSchool(configModel);
            DateTime nowSync = DateTime.Now;

            if (postSchool == null)
            {
                return new Result<AttendanceConfig>(null, "Đồng bộ thất bại!", false);
            }
            // kiểm tra xem trường đã có chưa nếu chưa thì thêm mới
            var checkSchoolAdd = await _organizationRepository.GetByFirstAsync(x => x.ReferenceId == postSchool.Id);
            if (checkSchoolAdd.Data == null)
            {
                checkSchoolAdd = await _organizationRepository.AddAsync(
                    new Organization()
                    {
                        ReferenceId = postSchool.Id,
                        OrganizationCode = postSchool.Code,
                        OrganizationName = postSchool.Name,
                        OrganizationAddress = postSchool.Address,
                        OrganizationTax = postSchool.TaxCode,
                        OrganizationPhone = postSchool.PhoneNumber,
                        OrganizationFax = postSchool.Fax,
                        OrganizationEmail = postSchool.Email,
                        OrganizationDescription = postSchool.Description,
                        OrganizationTypeId = postSchool.TypeCode,

                        ProvinceCode = postSchool.ProvinceCode,
                        ProvinceName = postSchool.ProvinceName,
                    }
                );

                //thêm vào time config
                if (checkSchoolAdd.Data != null)
                {
                    await _timeConfigRepository.AddAsync(new TimeConfig() { OrganizationId = checkSchoolAdd.Data.Id });
                }

            }
            else
            {

                Organization checkSchool = checkSchoolAdd.Data;

                checkSchool.ProvinceCode = postSchool.ProvinceCode;
                checkSchool.ProvinceName = postSchool.ProvinceName;
                checkSchool.LastModifiedDate = nowSync;
                await _organizationRepository.UpdateAsync(checkSchool);
            }

            if (checkSchoolAdd.Data == null)
            {
                return new Result<AttendanceConfig>(null, "Đồng bộ thất bại!", false);
            }


            //Gắn trường với attendance config
            var configAsync = await _appConfigRepository.GetByIdAsync(configModel.Id);
            configAsync.Data.OrganizationId = checkSchoolAdd.Data.Id;
            configAsync.Data.ReferenceId = checkSchoolAdd.Data.ReferenceId;
            configAsync.Data.TimeAsync = nowSync;


            var result = await _appConfigRepository.UpdateAsync(configAsync.Data);

            return result;
        }
        catch (Exception ex)
        {
            return new Result<AttendanceConfig>(null, $"Có lỗi: {ex.Message}", false);
        }
    }
    public async Task<Result<AttendanceConfig>> SaveAsync(AppConfigRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                //var check = await _appConfigRepository.GetByFirstAsync(x  => x.AccountName.Trim() == request.AccountName.Trim());
                //if (check.Data != null)
                //{
                //    return new Result<AttendanceConfig>(null, $"Tài khoản đã có vui lòng sử dụng tài khoản khác!", false);
                //}    

                var dataAdd = _mapper.Map<AttendanceConfig>(request); 
                var retVal = await _appConfigRepository.AddAsync(dataAdd);
                return retVal;
            }
            else
            {
                var data = await _appConfigRepository.GetByIdAsync(request.Id);
                var dataUpdate = data.Data;
                dataUpdate.EndpointGateway = request.EndpointGateway;
                dataUpdate.KeyIV = request.KeyIV;
                dataUpdate.Key = request.Key;
                dataUpdate.SecretKey = request.SecretKey;
                //dataUpdate.EndpointIdentity = request.EndpointIdentity;
                //dataUpdate.AccountName = request.AccountName;
                //dataUpdate.Password = request.Password;
                //dataUpdate.GrantType = request.GrantType;
                //dataUpdate.Scope = request.Scope;
                //dataUpdate.ClientId = request.ClientId;
                //dataUpdate.ClientSecret = request.ClientSecret;

    var retVal = await _appConfigRepository.UpdateAsync(dataUpdate);
                return retVal;
            }
        }
        catch (Exception ex)
        {
            return new Result<AttendanceConfig>(null, $"Lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<int>> DeleteAsync(DeleteRequest request)
    {
        try
        {
            var item = await _appConfigRepository.GetByIdAsync(request.Id);
            var retVal = await _appConfigRepository.DeleteAsync(request);

            return new Result<int>(1, retVal.Message, true);
        }
        catch (Exception ex)
        {
            return new Result<int>(0, $"Lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<AppeConfigResponse>>> Gets()
    {
        try
        {
            var retVal = await _appConfigRepository.GetAllAsync();

            var listMap = _mapper.Map<List<AppeConfigResponse>>(retVal);
            return new Result<List<AppeConfigResponse>>(listMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<List<AppeConfigResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<AppeConfigResponse>>> GetsFilter(AppConfigFilter filter)
    {
        try
        {
            var retVal = await (from cf in _dBContext.AttendanceConfig
                                join o in _dBContext.Organization on cf.OrganizationId equals o.Id into orgGroup
                                from o in orgGroup.DefaultIfEmpty() // LEFT JOIN
                                where (cf.Actived == true
                                //&& (!string.IsNullOrEmpty(filter.OrganizationId) && filter.OrganizationId != "0") ? cf.OrganizationId == filter.OrganizationId : true
                                  //&& ((filter.SiteId != 0) ? siteIds.Contains(device.SiteId) : true)
                                  //&& ((!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "serial_number") ? device.SerialNumber.ToLower().Contains(filter.Key.ToLower()) : true)
                                  //&& ((!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "device_name") ? device.DeviceName.ToLower().Contains(filter.Key.ToLower()) : true)
                                  )
                                select new AppeConfigResponse(cf, o)
                    )
                    .ToListAsync();

            return new Result<List<AppeConfigResponse>>(retVal, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<List<AppeConfigResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<AppeConfigResponse>> GetFirstOrDefault(string orgId)
    {
        try
        {
            var retVal = await _appConfigRepository.GetByFirstAsync(x => x.Actived == true && x.OrganizationId == orgId);

            var itemMap = _mapper.Map<AppeConfigResponse>(retVal.Data);
            return new Result<AppeConfigResponse>(itemMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<AppeConfigResponse>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

}
