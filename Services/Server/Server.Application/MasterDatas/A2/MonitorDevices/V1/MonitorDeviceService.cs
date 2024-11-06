using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Application.MasterDatas.A2.MonitorDevices.V1.Models;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Devices;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Application.MasterDatas.A2.MonitorDevices.V1;

public class MonitorDeviceService
{
    private readonly IMapper _mapper;
    private readonly IDeviceRepository _deviceRepository;
    private readonly MasterDataDbContext _dbContext;
    public MonitorDeviceService(IDeviceRepository deviceRepository, IMapper mapper, MasterDataDbContext dbContext)
    {
        _deviceRepository = deviceRepository;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Result<List<MDeviceResponse>>> Gets()
    {
        try
        {
            var devices = (await _deviceRepository.GetAllAsync()).Where(x => x.Actived == true).ToList();
            var devicesMap = _mapper.Map<List<MDeviceResponse>>(devices);
            return new Result<List<MDeviceResponse>>(devicesMap, $"Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<MDeviceResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<MDeviceResponse>>> GetsFilter(MDeviceFilter filter)
    {
        try
        {
            var devices = (await _deviceRepository.GetAllAsync())
                            .Where(x => x.Actived == true
                                    && ((!string.IsNullOrEmpty(filter.OnlineStatus) && filter.OnlineStatus == "Online") ? x.ConnectionStatus == true : true)   
                                    && ((!string.IsNullOrEmpty(filter.OnlineStatus) && filter.OnlineStatus == "Offline") ? x.ConnectionStatus != true : true)   
                                    && ((!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "serial_number") ? x.SerialNumber.Contains(filter.Key) : true)
                                    && ((!string.IsNullOrEmpty(filter.ColumnTable) && filter.ColumnTable == "device_name") ? x.DeviceName.Contains(filter.Key) : true)
                                    && ((!string.IsNullOrEmpty(filter.DeviceModel)) ? x.DeviceModel == filter.DeviceModel : true)
                            )
                            . OrderBy(x => x.ConnectionStatus)
                            .ThenBy(x => x.DeviceName)
                            .ToList();
            var devicesMap = _mapper.Map<List<MDeviceResponse>>(devices);
            return new Result<List<MDeviceResponse>>(devicesMap, $"Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<MDeviceResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<A2_Device>>> UpdateStatusConnect(List<MDeviceStatusRequest> requests)
    {
        try
        {
            var dataUpdate = new List<A2_Device>();
            foreach (var request in requests)
            {
                var deviceUpdate = await _dbContext.A2_Device.FirstOrDefaultAsync(x => x.SerialNumber == request.SerialNumber);
                if (deviceUpdate != null)
                {
                    deviceUpdate.CheckConnectTime = request.ConnectUpdateTime;
                    if (request.ConnectionStatus == true)
                    {
                        deviceUpdate.ConnectionStatus = true;
                        deviceUpdate.ConnectUpdateTime = request.ConnectUpdateTime;
                    }
                    else
                    {
                        deviceUpdate.ConnectionStatus = false;
                        deviceUpdate.DisConnectUpdateTime = request.ConnectUpdateTime;
                    }
                    dataUpdate.Add(deviceUpdate);
                }
            }

             _dbContext.A2_Device.UpdateRange(dataUpdate);
            var check = _dbContext.SaveChanges();
            if (check > 0)
            {
                return new Result<List<A2_Device>>(dataUpdate, $"Thành công!", true);
            }
            else
            {
                return new Result<List<A2_Device>>(null, $"Có lỗi xảy ra!", false);
            }    
        }
        catch (Exception e)
        {
            return new Result<List<A2_Device>>(null, $"Có lỗi: {e.Message}", false);
        }
        
    } 
}
