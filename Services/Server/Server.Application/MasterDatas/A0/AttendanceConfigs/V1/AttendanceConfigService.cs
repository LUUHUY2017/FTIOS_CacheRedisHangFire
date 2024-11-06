using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Application.MasterDatas.A0.AttendanceConfigs.V1.Models;
using Server.Application.Services.VTSmart;
using Server.Application.Services.VTSmart.Responses;
using Server.Core.Entities.A0;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A0;
using Server.Core.Interfaces.A2.Organizations;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;
using Shared.Core.Loggers;
using System.Text;

namespace Server.Application.MasterDatas.A0.AttendanceConfigs.V1;

public class AttendanceConfigService
{
    private readonly IAttendanceConfigRepository _attendanceConfigRepository;
    private readonly IMapper _mapper;
    private readonly SmartService _smartService;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly ITimeConfigRepository _timeConfigRepository;
    private readonly IMasterDataDbContext _dBContext;
    public AttendanceConfigService(
        IAttendanceConfigRepository attendanceConfigRepository,
        IMapper mapper,
        SmartService smartService,
        IOrganizationRepository organizationRepository,
        ITimeConfigRepository timeConfigRepository,
        IMasterDataDbContext dBContext
        )
    {
        _attendanceConfigRepository = attendanceConfigRepository;
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
    public async Task<AccessTokenLocal> GetToken(A0_AttendanceConfig configModel)
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

    public async Task<SchoolResponse> PostSchool(A0_AttendanceConfig configModel)
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

    public async Task<Result<A0_AttendanceConfig>> SchoolAsync(A0_AttendanceConfig configModel)
    {
        try
        {
            // lấy trường theo config
            var postSchool = await PostSchool(configModel);
            DateTime nowSync = DateTime.Now;

            if (postSchool == null)
            {
                return new Result<A0_AttendanceConfig>(null, "Đồng bộ thất bại!", false);
            }
            // kiểm tra xem trường đã có chưa nếu chưa thì thêm mới
            var checkSchoolAdd = await _organizationRepository.GetByFirstAsync(x => x.ReferenceId == postSchool.Id);
            if (checkSchoolAdd.Data == null)
            {
                checkSchoolAdd = await _organizationRepository.AddAsync(
                    new A2_Organization()
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
                    await _timeConfigRepository.AddAsync(new A0_TimeConfig() { OrganizationId = checkSchoolAdd.Data.Id });
                }

            }
            else
            {

                A2_Organization checkSchool = checkSchoolAdd.Data;

                checkSchool.ProvinceCode = postSchool.ProvinceCode;
                checkSchool.ProvinceName = postSchool.ProvinceName;
                checkSchool.LastModifiedDate = nowSync;
                await _organizationRepository.UpdateAsync(checkSchool);
            }

            if (checkSchoolAdd.Data == null)
            {
                return new Result<A0_AttendanceConfig>(null, "Đồng bộ thất bại!", false);
            }


            //Gắn trường với attendance config
            var configAsync = await _attendanceConfigRepository.GetByIdAsync(configModel.Id);
            configAsync.Data.OrganizationId = checkSchoolAdd.Data.Id;
            configAsync.Data.ReferenceId = checkSchoolAdd.Data.ReferenceId;
            configAsync.Data.TimeAsync = nowSync;


            var result = await _attendanceConfigRepository.UpdateAsync(configAsync.Data);

            return result;
        }
        catch (Exception ex)
        {
            return new Result<A0_AttendanceConfig>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<A0_AttendanceConfig>> SaveAsync(AttendanceConfigRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var check = await _attendanceConfigRepository.GetByFirstAsync(x => x.AccountName.Trim() == request.AccountName.Trim());
                if (check.Data != null)
                {
                    return new Result<A0_AttendanceConfig>(null, $"Tài khoản đã có vui lòng sử dụng tài khoản khác!", false);
                }

                var dataAdd = _mapper.Map<A0_AttendanceConfig>(request);
                var retVal = await _attendanceConfigRepository.AddAsync(dataAdd);
                return retVal;
            }
            else
            {
                var data = await _attendanceConfigRepository.GetByIdAsync(request.Id);
                var dataUpdate = data.Data;
                dataUpdate.EndpointIdentity = request.EndpointIdentity;
                dataUpdate.EndpointGateway = request.EndpointGateway;
                dataUpdate.AccountName = request.AccountName;
                dataUpdate.Password = request.Password;
                dataUpdate.GrantType = request.GrantType;
                dataUpdate.Scope = request.Scope;
                dataUpdate.ClientId = request.ClientId;
                dataUpdate.ClientSecret = request.ClientSecret;

                var retVal = await _attendanceConfigRepository.UpdateAsync(dataUpdate);
                return retVal;
            }
        }
        catch (Exception ex)
        {
            return new Result<A0_AttendanceConfig>(null, $"Lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<int>> DeleteAsync(DeleteRequest request)
    {
        try
        {
            var item = await _attendanceConfigRepository.GetByIdAsync(request.Id);
            var retVal = await _attendanceConfigRepository.DeleteAsync(request);

            return new Result<int>(1, retVal.Message, true);
        }
        catch (Exception ex)
        {
            return new Result<int>(0, $"Lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<AttendanceConfigResponse>>> Gets()
    {
        try
        {
            var retVal = await _attendanceConfigRepository.GetAllAsync();

            var listMap = _mapper.Map<List<AttendanceConfigResponse>>(retVal);
            return new Result<List<AttendanceConfigResponse>>(listMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<List<AttendanceConfigResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<AttendanceConfigResponse>>> GetsFilter(AttendanceFilter filter)
    {
        try
        {
            var retVal = await (from cf in _dBContext.A0_AttendanceConfig
                                join o in _dBContext.A2_Organization on cf.OrganizationId equals o.Id into orgGroup
                                from o in orgGroup.DefaultIfEmpty() // LEFT JOIN
                                where (cf.Actived == true
                                && (!string.IsNullOrEmpty(filter.OrganizationId) && filter.OrganizationId != "0") ? cf.OrganizationId == filter.OrganizationId : true
                                  //&& ((filter.SiteId != 0) ? siteIds.Contains(device.SiteId) : true)
                                  //&& ((!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "serial_number") ? device.SerialNumber.ToLower().Contains(filter.Key.ToLower()) : true)
                                  //&& ((!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "device_name") ? device.DeviceName.ToLower().Contains(filter.Key.ToLower()) : true)
                                  )
                                select new AttendanceConfigResponse(cf, o)
                    )
                    .ToListAsync();

            return new Result<List<AttendanceConfigResponse>>(retVal, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<List<AttendanceConfigResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<AttendanceConfigResponse>> GetFirstOrDefault(string orgId)
    {
        try
        {
            var retVal = await _attendanceConfigRepository.GetByFirstAsync(x => x.Actived == true && x.OrganizationId == orgId);

            var itemMap = _mapper.Map<AttendanceConfigResponse>(retVal.Data);
            return new Result<AttendanceConfigResponse>(itemMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<AttendanceConfigResponse>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

}
