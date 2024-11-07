using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Server.Application.MasterDatas.A0.AttendanceConfigs.V1.Models;
using Server.Application.MasterDatas.A0.TimeConfigs.V1.Models;
using Server.Application.MasterDatas.A2.Organizations.V1;
using Server.Core.Entities.A0;
using Server.Core.Interfaces.A0;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;
using Shared.Core.Identity.Object;

namespace Server.Application.MasterDatas.A0.TimeConfigs.V1;

public class TimeConfigService
{
    public string? UserId { get; set; } 
    private readonly ITimeConfigRepository _timeConfigRepository;
    private readonly IMapper _mapper;
    private readonly IMasterDataDbContext _dBContext;
    private readonly OrganizationService _organizationService;
    public TimeConfigService(
        ITimeConfigRepository timeConfigRepository, 
        IMapper mapper,
        IMasterDataDbContext dBContext,
        OrganizationService organizationService
        )
    {
        _timeConfigRepository = timeConfigRepository;
        _mapper = mapper;
        _dBContext = dBContext;
        _organizationService = organizationService;
    }

    public async Task<Result<A0_TimeConfig>> SaveAsync(TimeConfigRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var dataAdd = _mapper.Map<A0_TimeConfig>(request);
                var retVal = await _timeConfigRepository.AddAsync(dataAdd);
                return retVal;
            }
            else
            {
                var data = await _timeConfigRepository.GetByIdAsync(request.Id);
                var dataUpdate = data.Data;
                //dataUpdate.OrganizationId = request.OrganizationId;
                dataUpdate.MorningStartTime = request.MorningStartTime;
                dataUpdate.MorningEndTime = request.MorningEndTime;
                dataUpdate.MorningLateTime = request.MorningLateTime;
                dataUpdate.MorningBreakTime = request.MorningBreakTime;
                dataUpdate.AfternoonStartTime = request.AfternoonStartTime;
                dataUpdate.AfternoonEndTime = request.AfternoonEndTime;
                dataUpdate.AfternoonLateTime = request.AfternoonLateTime;
                dataUpdate.AfternoonBreakTime = request.AfternoonBreakTime;
                dataUpdate.EveningStartTime = request.EveningStartTime;
                dataUpdate.EveningEndTime = request.EveningEndTime;
                dataUpdate.EveningLateTime = request.EveningLateTime;
                dataUpdate.EveningBreakTime = request.EveningBreakTime;

                var retVal = await _timeConfigRepository.UpdateAsync(dataUpdate);
                return retVal;
            }
        }
        catch (Exception ex)
        {
            return new Result<A0_TimeConfig>(null, $"Lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<TimeConfigResponse>> DeleteAsync(DeleteRequest request)
    {
        try
        {
            var item = await _timeConfigRepository.GetByIdAsync(request.Id);
            var retVal = await _timeConfigRepository.DeleteAsync(request);
            var itemMap = _mapper.Map<TimeConfigResponse>(item);

            return new Result<TimeConfigResponse>(itemMap, retVal.Message, false);
        }
        catch (Exception ex)
        {
            return new Result<TimeConfigResponse>(null, $"Lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<TimeConfigResponse>>> Gets()
    {
        try
        {
            var retVal = await _timeConfigRepository.GetAllAsync();

            var listMap = _mapper.Map<List<TimeConfigResponse>>(retVal);
            return new Result<List<TimeConfigResponse>>(listMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<List<TimeConfigResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<TimeConfigResponse>>> GetsFilter(TimeConfigFilter filter)
    {
        try
        {
            _organizationService.UserId = UserId;
            var userOrgIds = (await _organizationService.GetForUser()).Data.Select(x => x.Id).ToList();
            var retVal = await (from o in _dBContext.A2_Organization
                                join tc in _dBContext.A0_TimeConfig on o.Id equals tc.OrganizationId into orgGroup
                                from tc in orgGroup.DefaultIfEmpty() // LEFT JOIN
                                where (tc.Actived == true
                                  && (!string.IsNullOrEmpty(filter.OrganizationId) && filter.OrganizationId != "0") ? tc.OrganizationId == filter.OrganizationId : true
                                  //&& ((filter.SiteId != 0) ? siteIds.Contains(device.SiteId) : true)
                                  //&& ((!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "serial_number") ? device.SerialNumber.ToLower().Contains(filter.Key.ToLower()) : true)
                                  //&& ((!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "device_name") ? device.DeviceName.ToLower().Contains(filter.Key.ToLower()) : true)
                                  )
                                select new TimeConfigResponse(tc, o)
                    )
                    .ToListAsync();

            retVal = retVal.Where(x => userOrgIds.Contains(x.OrganizationId)).ToList();
            return new Result<List<TimeConfigResponse>>(retVal, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<List<TimeConfigResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<TimeConfigResponse>> GetFirstOrDefault(string orgId)
    {
        try
        {
            var retVal = await _timeConfigRepository.GetByFirstAsync(x => x.Actived == true && x.OrganizationId == orgId);

            var itemMap = _mapper.Map<TimeConfigResponse>(retVal.Data);
            return new Result<TimeConfigResponse>(itemMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<TimeConfigResponse>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

}
