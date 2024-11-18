using AMMS.DeviceData.RabbitMq;
using AMMS.Hanet.Applications.TerminalCommandLogs.V1.Models;
using AMMS.Hanet.Applications.V1.Service;
using AMMS.Hanet.Datas.Databases;
using AMMS.Hanet.Datas.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Core.Commons;

namespace AMMS.Hanet.Applications.TerminalCommandLogs.V1;

public class TerminalCommandLogService
{
    private readonly IMapper _mapper;
    private readonly IDeviceAutoPushDbContext _dbContext;
    private readonly HANET_Server_Push_Service _hanetAPIService;

    public TerminalCommandLogService(IMapper mapper, IDeviceAutoPushDbContext dbContext, HANET_Server_Push_Service hANET_API_Service)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _hanetAPIService = hANET_API_Service;
    }

    public async Task<Result<List<hanet_commandlog>>> Gets(TerminalCommandLogFilter filter)
    {
        try
        {
            var status = filter.Status == "1";
            var data = await _dbContext.hanet_commandlog.Where(x =>
                                                        ((!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "serial_number") ? x.terminal_sn.Contains(filter.Key) : true)
                                                        && (!string.IsNullOrEmpty(filter.Status) ? x.successed == status : true)
                                                        && (filter.StartDate != null ? x.create_time >= filter.StartDate : true)
                                                        && (filter.EndDate != null ? x.create_time <= filter.EndDate.Value.Date.AddDays(1).AddMilliseconds(-1) : true)
                                                        ).ToListAsync();
            return new Result<List<hanet_commandlog>>(data, $"Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<hanet_commandlog>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<hanet_commandlog>> GetDetail(string id)
    {
        try
        {
            var data = await _dbContext.hanet_commandlog.FirstOrDefaultAsync(x => x.Id == id);
            return new Result<hanet_commandlog>(data, $"Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<hanet_commandlog>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<int>> Delete(DeleteRequest request)
    {
        try
        {
            var deleteModel = await _dbContext.hanet_commandlog.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (deleteModel == null)
            {
                return new Result<int>(0, $"Không tìm thấy", false);
            }

            _dbContext.hanet_commandlog.Remove(deleteModel);
            var check = await _dbContext.SaveChangesAsync();
            if (check > 0)
            {
                return new Result<int>(1, $"Thành công!", true);
            }
            return new Result<int>(0, $"Có lỗi xảy ra!", false);
        }
        catch (Exception ex)
        {
            return new Result<int>(0, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<int>> Resend(DeleteRequest request)
    {
        try
        {
            var conmandlog = await _dbContext.hanet_commandlog.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (conmandlog == null)
            {
                return new Result<int>(0, $"Không tìm thấy", false);
            }

            if (string.IsNullOrEmpty(conmandlog.content))
                return new Result<int>(0, $"Không có dữ liệu", false);

            if (conmandlog.command_ation == ServerRequestAction.ActionAdd && conmandlog.command_type == ServerRequestType.UserInfo)
                await _hanetAPIService.UpdatePerson(conmandlog);


            return new Result<int>(0, $"Đẩy lại thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<int>(0, $"Có lỗi: {ex.Message}", false);
        }
    }

}
