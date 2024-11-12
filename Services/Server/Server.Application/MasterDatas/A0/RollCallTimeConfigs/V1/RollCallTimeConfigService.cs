using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Application.MasterDatas.A0.RollCallTimeConfigs.V1.Models;
using Server.Application.MasterDatas.A2.Organizations.V1;
using Server.Core.Entities.A0;
using Server.Core.Interfaces.A0;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Application.MasterDatas.A0.RollCallTimeConfigs.V1;

public class RollCallTimeConfigService
{
    public string? UserId { get; set; }
    private readonly IRollCallTimeConfigRepository _rollCallTimeConfigRepository;
    private readonly IMapper _mapper;
    private readonly IMasterDataDbContext _dBContext;
    private readonly OrganizationService _organizationService;
    public RollCallTimeConfigService(
        IRollCallTimeConfigRepository rollCallTimeConfigRepository,
        IMapper mapper,
        IMasterDataDbContext dBContext,
        OrganizationService organizationService
        )
    {
        _rollCallTimeConfigRepository = rollCallTimeConfigRepository;
        _mapper = mapper;
        _dBContext = dBContext;
        _organizationService = organizationService;
    }

    public async Task<Result<RollCallTimeConfig>> SaveAsync(RollCallTimeConfigRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var dataAdd = _mapper.Map<RollCallTimeConfig>(request);
                var retVal = await _rollCallTimeConfigRepository.AddAsync(dataAdd);
                return retVal;
            }
            else
            {
                var data = await _rollCallTimeConfigRepository.GetByIdAsync(request.Id);
                var dataUpdate = data.Data;
                dataUpdate.OrganizationId = request.OrganizationId;
                //dataUpdate.OrganizationName = request.OrganizationName;
                dataUpdate.RollCallName = request.RollCallName;
                dataUpdate.StartTime = request.StartTime;
                dataUpdate.EndTime = request.EndTime;
                dataUpdate.Note = request.Note;
                var retVal = await _rollCallTimeConfigRepository.UpdateAsync(dataUpdate);
                return retVal;
            }
        }
        catch (Exception ex)
        {
            return new Result<RollCallTimeConfig>(null, $"Lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<int>> DeleteAsync(DeleteRequest request)
    {
        try
        {
            var item = await _rollCallTimeConfigRepository.GetByIdAsync(request.Id);
            var retVal = await _rollCallTimeConfigRepository.DeleteAsync(request);
            //var itemMap = _mapper.Map<RollCallTimeConfigResponse>(item.Data);

            return retVal;
        }
        catch (Exception ex)
        {
            return new Result<int>(0, $"Lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<RollCallTimeConfigResponse>>> Gets()
    {
        try
        {
            var retVal = await _rollCallTimeConfigRepository.GetAllAsync();

            var listMap = _mapper.Map<List<RollCallTimeConfigResponse>>(retVal);
            return new Result<List<RollCallTimeConfigResponse>>(listMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<List<RollCallTimeConfigResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<RollCallTimeConfigResponse>>> GetsFilter(RollCallTimeConfigFilter filter)
    {
        try
        {
            _organizationService.UserId = UserId;

            var retVal = await (from tc in _dBContext.RollCallTimeConfig
                                join o in _dBContext.Organization on tc.OrganizationId equals o.Id
                                where o.Actived == true
                                  && !string.IsNullOrEmpty(filter.OrganizationId) && filter.OrganizationId != "0" ? tc.OrganizationId == filter.OrganizationId : true
                                //&& ((filter.SiteId != 0) ? siteIds.Contains(device.SiteId) : true)
                                //&& ((!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "serial_number") ? device.SerialNumber.ToLower().Contains(filter.Key.ToLower()) : true)
                                //&& ((!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "device_name") ? device.DeviceName.ToLower().Contains(filter.Key.ToLower()) : true)

                                select new RollCallTimeConfigResponse(tc, o)
                    )
                    .ToListAsync();


            //retVal = retVal.Where(x => userOrgIds.Contains(x.Id)).ToList();
            return new Result<List<RollCallTimeConfigResponse>>(retVal, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<List<RollCallTimeConfigResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<RollCallTimeConfigResponse>> GetFirstOrDefault(string orgId)
    {
        try
        {
            var retVal = await _rollCallTimeConfigRepository.GetByFirstAsync(x => x.Actived == true && x.OrganizationId == orgId);

            var itemMap = _mapper.Map<RollCallTimeConfigResponse>(retVal.Data);
            return new Result<RollCallTimeConfigResponse>(itemMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<RollCallTimeConfigResponse>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

}
