using AMMS.Hanet.Applications.TerminalCommandLogs.V1.Models;
using AMMS.Hanet.Datas.Databases;
using AMMS.Hanet.Datas.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.Hanet.Applications.TerminalCommandLogs.V1;

public class TerminalCommandLogService
{
    private readonly IMapper _mapper;
    private readonly IDeviceAutoPushDbContext _dbContext;
    public TerminalCommandLogService(IMapper mapper, IDeviceAutoPushDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Result<List<hanet_commandlog>>> Gets(TerminalCommandLogFilter filter)
    {
        try
        {
            var data = await _dbContext.hanet_commandlog.Where(x => true).ToListAsync();
            return new Result<List<hanet_commandlog>>(data, $"Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<hanet_commandlog>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    //public async Task<Result<List<hanet_commandlog>>> Gets(TerminalCommandLogFilter filter)
    //{
    //    try
    //    {
    //        var data = await _dbContext.hanet_commandlog.Where(x => true).ToListAsync();
    //        return new Result<List<hanet_commandlog>>(data, $"Thành công!", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        return new Result<List<hanet_commandlog>>(null, $"Có lỗi: {ex.Message}", false);
    //    }
    //}
}
