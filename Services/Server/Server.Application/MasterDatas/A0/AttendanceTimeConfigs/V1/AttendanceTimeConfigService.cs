using AMMS.DeviceData.RabbitMq;
using AutoMapper;
using EventBus.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Application.MasterDatas.A0.AttendanceTimeConfigs.V1.Models;
using Server.Application.MasterDatas.A2.Organizations.V1;
using Server.Core.Entities.A0;
using Server.Core.Interfaces.A0;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Application.MasterDatas.A0.AttendanceTimeConfigs.V1;

public class AttendanceTimeConfigService
{
    public string? UserId { get; set; }
    private readonly IAttendanceTimeConfigRepository _attendanceTimeConfigRepository;
    private readonly IMapper _mapper;
    private readonly IMasterDataDbContext _dBContext;
    private readonly OrganizationService _organizationService;

    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly IConfiguration _configuration;


    public AttendanceTimeConfigService(
        IMapper mapper,
        IConfiguration configuration,
        IEventBusAdapter eventBusAdapter,
        IAttendanceTimeConfigRepository AttendanceTimeConfigRepository,
        IMasterDataDbContext dBContext,
        OrganizationService organizationService
        )
    {
        _mapper = mapper;
        _dBContext = dBContext;
        _configuration = configuration;
        _eventBusAdapter = eventBusAdapter;
        _attendanceTimeConfigRepository = AttendanceTimeConfigRepository;
        _organizationService = organizationService;
    }

    public async Task<Result<AttendanceTimeConfig>> SaveAsync(AttendanceTimeConfigRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var dataAdd = _mapper.Map<AttendanceTimeConfig>(request);
                var retVal = await _attendanceTimeConfigRepository.AddAsync(dataAdd);
                return retVal;
            }
            else
            {
                var data = await _attendanceTimeConfigRepository.GetByIdAsync(request.Id);
                var dataUpdate = data.Data;
                dataUpdate.OrganizationId = request.OrganizationId;
                //dataUpdate.OrganizationName = request.OrganizationName;
                dataUpdate.Name = request.Name;
                dataUpdate.Type = request.Type;
                dataUpdate.StartTime = request.StartTime;
                dataUpdate.EndTime = request.EndTime;
                dataUpdate.LateTime = request.LateTime;
                dataUpdate.BreakTime = request.BreakTime;
                dataUpdate.Note = request.Note;
                var retVal = await _attendanceTimeConfigRepository.UpdateAsync(dataUpdate);
                return retVal;
            }

        }
        catch (Exception ex)
        {
            return new Result<AttendanceTimeConfig>(null, $"Lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<int>> DeleteAsync(DeleteRequest request)
    {
        try
        {
            var item = await _attendanceTimeConfigRepository.GetByIdAsync(request.Id);
            var retVal = await _attendanceTimeConfigRepository.DeleteAsync(request);
            //var itemMap = _mapper.Map<AttendanceTimeConfigResponse>(item.Data);

            return retVal;
        }
        catch (Exception ex)
        {
            return new Result<int>(0, $"Lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<AttendanceTimeConfigResponse>>> Gets()
    {
        try
        {
            var retVal = await _attendanceTimeConfigRepository.GetAllAsync();

            var listMap = _mapper.Map<List<AttendanceTimeConfigResponse>>(retVal);
            return new Result<List<AttendanceTimeConfigResponse>>(listMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<List<AttendanceTimeConfigResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<AttendanceTimeConfigResponse>>> GetsFilter(AttendanceTimeConfigFilter filter)
    {
        try
        {
            _organizationService.UserId = UserId;

            var retVal = await (from tc in _dBContext.AttendanceTimeConfig
                                join o in _dBContext.Organization
                                on tc.OrganizationId equals o.Id into orgGroup
                                from o in orgGroup.DefaultIfEmpty() // LEFT JOIN
                                where ((tc.Actived == filter.Actived)
                                  && ((!string.IsNullOrEmpty(filter.OrganizationId) && filter.OrganizationId != "0") ? tc.OrganizationId == filter.OrganizationId : true)
                                  && ((!string.IsNullOrEmpty(filter.Type)) ? tc.Type == filter.Type : true)
                                 )
                                orderby tc.Type
                                select new AttendanceTimeConfigResponse(tc, o)
                    )
                    .ToListAsync();


            //retVal = retVal.Where(x => userOrgIds.Contains(x.Id)).ToList();
            return new Result<List<AttendanceTimeConfigResponse>>(retVal, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<List<AttendanceTimeConfigResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<AttendanceTimeConfigResponse>> GetFirstOrDefault(string orgId)
    {
        try
        {
            var retVal = await _attendanceTimeConfigRepository.GetByFirstAsync(x => x.Actived == true && x.OrganizationId == orgId);

            var itemMap = _mapper.Map<AttendanceTimeConfigResponse>(retVal.Data);
            return new Result<AttendanceTimeConfigResponse>(itemMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<AttendanceTimeConfigResponse>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<AttendanceTimeConfig>> Active(ActiveRequest request)
    {
        try
        {
            var modelDel = await _attendanceTimeConfigRepository.GetByIdAsync(request.Id);
            if (modelDel == null)
                return new Result<AttendanceTimeConfig>(null, "Không tìm thấy dữ liệu!", false);

            var response = await _attendanceTimeConfigRepository.ActiveAsync(request);

            return response;
        }
        catch (Exception ex)
        {
            return new Result<AttendanceTimeConfig>(null, $"Lỗi: {ex.Message}", false);
        }
    }
    public async Task<Result<AttendanceTimeConfig>> InActive(InactiveRequest request)
    {
        try
        {
            var modelDel = await _attendanceTimeConfigRepository.GetByIdAsync(request.Id);
            if (modelDel == null)
                return new Result<AttendanceTimeConfig>(null, "Không tìm thấy dữ liệu!", false);

            var response = await _attendanceTimeConfigRepository.InactiveAsync(request);

            return response;
        }
        catch (Exception ex)
        {
            return new Result<AttendanceTimeConfig>(null, $"Lỗi: {ex.Message}", false);
        }
    }
    public async Task<Result<bool>> ChangeDataPushEventRb()
    {
        try
        {
            RB_DataResponse rB_Response = new RB_DataResponse()
            {
                Id = Guid.NewGuid().ToString(),
                Content = "Thay đổi thời gian",
                ReponseType = RB_DataResponseType.ChangeAttendenceTime,
            };
            var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Server_Auto_Push_SMAS}");
            await aa.Send(rB_Response);

            return new Result<bool>($"Thành công ", false);
        }
        catch (Exception ex)
        {
            return new Result<bool>($"Lỗi: {ex.Message}", false);
        }
    }


}
