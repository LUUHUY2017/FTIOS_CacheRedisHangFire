using AMMS.ZkAutoPush.Datas.Databases;
using AMMS.ZkAutoPush.Datas.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.ZkAutoPush.Applications.MonitorDevices.V1.Models;

public class MonitorDeviceService
{
    private readonly IMapper _mapper;
    private readonly IDeviceAutoPushDbContext _dbContext;

    public MonitorDeviceService(IMapper mapper, IDeviceAutoPushDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Result<List<zk_terminal>>> Gets(MDeviceFilter filter)
    {
        try
        {
            var status = filter.Status == "1";
            var data = await _dbContext.zk_terminal.Where(x =>
                                                            (!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "serial_number" ? x.sn.Contains(filter.Key) : true)
                                                            && (!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "device_name" ? x.name.Contains(filter.Key) : true)
                                                            && (!string.IsNullOrEmpty(filter.Status) ? x.online_status == status : true)
                                                    ).ToListAsync();
            return new Result<List<zk_terminal>>(data, "Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<zk_terminal>>(null, $"Có lỗi: {ex.Message}", false);
        }

    }

    public async Task<Result<int>> Delete(DeleteRequest request)
    {
        try
        {
            var deleteModel = await _dbContext.zk_terminal.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (deleteModel == null)
            {
                return new Result<int>(0, $"Không tìm thấy", false);
            }

            _dbContext.zk_terminal.Remove(deleteModel);
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
