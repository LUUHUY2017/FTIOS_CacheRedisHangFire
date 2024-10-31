using AMMS.Hanet.Datas.Databases;
using AMMS.Hanet.Datas.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.Hanet.Applications.MonitorDevices.V1;

public class MonitorDeviceService
{
    private readonly IMapper _mapper;
    private readonly IDeviceAutoPushDbContext _dbContext;

    public MonitorDeviceService(IMapper mapper, IDeviceAutoPushDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Result<List<hanet_terminal>>> Gets()
    {
        try
        {
            var data = await _dbContext.hanet_terminal.ToListAsync();
            return new Result<List<hanet_terminal>>(data, "Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<hanet_terminal>>(null, "Có lỗi xảy ra!", false);
        }

    }
}
