using AMMS.ZkAutoPush.Applications.TerminalCommandLogs.V1.Models;
using AMMS.ZkAutoPush.Datas.Databases;
using AMMS.ZkAutoPush.Datas.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.ZkAutoPush.Applications.TerminalCommandLogs.V1;

public class TerminalCommandLogService
{
    private readonly IMapper _mapper;
    private readonly IDeviceAutoPushDbContext _dbContext;
    public TerminalCommandLogService(IMapper mapper, IDeviceAutoPushDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Result<List<zk_terminalcommandlog>>> Gets(TerminalCommandLogFilter filter)
    {
        try
        {
            var status = filter.Status == "1";
            var data = await _dbContext.zk_terminalcommandlog.Where(x =>
                                                        (!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "serial_number" ? x.terminal_sn.Contains(filter.Key) : true)
                                                        && (!string.IsNullOrEmpty(filter.Status) ? x.successed == status : true)
                                                        ).ToListAsync();
            return new Result<List<zk_terminalcommandlog>>(data, $"Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<zk_terminalcommandlog>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<zk_terminalcommandlog>> GetDetail(string id)
    {
        try
        {
            var data = await _dbContext.zk_terminalcommandlog.FirstOrDefaultAsync(x => x.Id == id);
            return new Result<zk_terminalcommandlog>(data, $"Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<zk_terminalcommandlog>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<int>> Delete(DeleteRequest request)
    {
        try
        {
            var deleteModel = await _dbContext.zk_terminalcommandlog.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (deleteModel == null)
            {
                return new Result<int>(0, $"Không tìm thấy", false);
            }

            _dbContext.zk_terminalcommandlog.Remove(deleteModel);
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
}
