using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    private readonly IAttendanceTimeConfigRepository _AttendanceTimeConfigRepository;
    private readonly IMapper _mapper;
    private readonly IMasterDataDbContext _dBContext;
    private readonly OrganizationService _organizationService;
    public AttendanceTimeConfigService(
        IAttendanceTimeConfigRepository AttendanceTimeConfigRepository,
        IMapper mapper,
        IMasterDataDbContext dBContext,
        OrganizationService organizationService
        )
    {
        _AttendanceTimeConfigRepository = AttendanceTimeConfigRepository;
        _mapper = mapper;
        _dBContext = dBContext;
        _organizationService = organizationService;
    }

    public async Task<Result<AttendanceTimeConfig>> SaveAsync(AttendanceTimeConfigRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var dataAdd = _mapper.Map<AttendanceTimeConfig>(request);
                var retVal = await _AttendanceTimeConfigRepository.AddAsync(dataAdd);
                return retVal;
            }
            else
            {
                var data = await _AttendanceTimeConfigRepository.GetByIdAsync(request.Id);
                var dataUpdate = data.Data;
                dataUpdate.OrganizationId = request.OrganizationId;
                //dataUpdate.OrganizationName = request.OrganizationName;
                dataUpdate.RollCallName = request.RollCallName;
                dataUpdate.StartTime = request.StartTime;
                dataUpdate.EndTime = request.EndTime;
                dataUpdate.Note = request.Note;
                var retVal = await _AttendanceTimeConfigRepository.UpdateAsync(dataUpdate);
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
            var item = await _AttendanceTimeConfigRepository.GetByIdAsync(request.Id);
            var retVal = await _AttendanceTimeConfigRepository.DeleteAsync(request);
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
            var retVal = await _AttendanceTimeConfigRepository.GetAllAsync();

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
                                join o in _dBContext.Organization on tc.OrganizationId equals o.Id
                                where o.Actived == true
                                  && !string.IsNullOrEmpty(filter.OrganizationId) && filter.OrganizationId != "0" ? tc.OrganizationId == filter.OrganizationId : true
                                //&& ((filter.SiteId != 0) ? siteIds.Contains(device.SiteId) : true)
                                //&& ((!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "serial_number") ? device.SerialNumber.ToLower().Contains(filter.Key.ToLower()) : true)
                                //&& ((!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "device_name") ? device.DeviceName.ToLower().Contains(filter.Key.ToLower()) : true)

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
            var retVal = await _AttendanceTimeConfigRepository.GetByFirstAsync(x => x.Actived == true && x.OrganizationId == orgId);

            var itemMap = _mapper.Map<AttendanceTimeConfigResponse>(retVal.Data);
            return new Result<AttendanceTimeConfigResponse>(itemMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<AttendanceTimeConfigResponse>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

}
