using AMMS.ZkAutoPush.Applications.V1;
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
    private readonly DeviceCommandCacheService _deviceCommandCacheService;

    public MonitorDeviceService(IMapper mapper, IDeviceAutoPushDbContext dbContext, DeviceCommandCacheService deviceCommandCacheService)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _deviceCommandCacheService = deviceCommandCacheService;
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

    public async Task<Result<bool>> Reboot(ObjectString request)
    {
        try
        {
            var crrTerminal = await _dbContext.zk_terminal.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (crrTerminal == null)
            {
                return new Result<bool>(false, $"Không tìm thấy", false);
            }
            if (string.IsNullOrEmpty(crrTerminal.sn))
            {
                return new Result<bool>(false, $"Không có thông tin Serialnumber", false);
            }

            var sn = crrTerminal.sn;

            var command = new IclockCommand();

            var id = DateTime.Now.TimeOfDay.Ticks;

            command = IclockOperarion.RebootDevice(id, sn);

            if (command != null)
            {
                await _deviceCommandCacheService.Save(command);
                return new Result<bool>(true, $"Gửi lệnh khởi động lại thành công!", true);
            }
            return new Result<bool>(false, $"Có lỗi xảy ra!", false);
        }
        catch (Exception ex)
        {
            return new Result<bool>(false, $"Có lỗi: {ex.Message}", false);
        }
    }
    public async Task<Result<bool>> DeleteAllLog(ObjectString request)
    {
        try
        {
            var crrTerminal = await _dbContext.zk_terminal.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (crrTerminal == null)
            {
                return new Result<bool>(false, $"Không tìm thấy", false);
            }
            if (string.IsNullOrEmpty(crrTerminal.sn))
            {
                return new Result<bool>(false, $"Không có thông tin Serialnumber", false);
            }

            var sn = crrTerminal.sn;

            var command = new IclockCommand();

            var id = DateTime.Now.TimeOfDay.Ticks;

            command = IclockOperarion.ClearAttData(id, sn);

            if (command != null)
            {
                await _deviceCommandCacheService.Save(command);
                return new Result<bool>(true, $"Gửi lệnh xoá lịch sử thành công!", true);
            }
            return new Result<bool>(false, $"Có lỗi xảy ra!", false);
        }
        catch (Exception ex)
        {
            return new Result<bool>(false, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<bool>> DeleteAllFace(ObjectString request)
    {
        try
        {
            var crrTerminal = await _dbContext.zk_terminal.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (crrTerminal == null)
            {
                return new Result<bool>(false, $"Không tìm thấy", false);
            }
            if (string.IsNullOrEmpty(crrTerminal.sn))
            {
                return new Result<bool>(false, $"Không có thông tin Serialnumber", false);
            }

            var sn = crrTerminal.sn;

            var command = new IclockCommand();

            var id = DateTime.Now.TimeOfDay.Ticks;

            command = IclockOperarion.ClearPhotoData(id, sn);

            if (command != null)
            {
                await _deviceCommandCacheService.Save(command);
                return new Result<bool>(true, $"Gửi lệnh xoá khuôn mặt thành công!", true);
            }
            return new Result<bool>(false, $"Có lỗi xảy ra!", false);
        }
        catch (Exception ex)
        {
            return new Result<bool>(false, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<bool>> DeleteAllData(ObjectString request)
    {
        try
        {
            var crrTerminal = await _dbContext.zk_terminal.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (crrTerminal == null)
            {
                return new Result<bool>(false, $"Không tìm thấy", false);
            }
            if (string.IsNullOrEmpty(crrTerminal.sn))
            {
                return new Result<bool>(false, $"Không có thông tin Serialnumber", false);
            }

            var sn = crrTerminal.sn;

            var command = new IclockCommand();

            var id = DateTime.Now.TimeOfDay.Ticks;

            command = IclockOperarion.ClearData(id, sn);

            if (command != null)
            {
                await _deviceCommandCacheService.Save(command);
                return new Result<bool>(true, $"Gửi lệnh xoá tất cả dữ liệu thành công!", true);
            }
            return new Result<bool>(false, $"Có lỗi xảy ra!", false);
        }
        catch (Exception ex)
        {
            return new Result<bool>(false, $"Có lỗi: {ex.Message}", false);
        }
    }

}
