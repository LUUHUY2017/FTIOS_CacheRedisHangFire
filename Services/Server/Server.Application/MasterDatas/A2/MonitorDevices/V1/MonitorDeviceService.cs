using AutoMapper;
using Server.Application.MasterDatas.A2.MonitorDevices.V1.Models;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Devices;
using Shared.Core.Commons;

namespace Server.Application.MasterDatas.A2.MonitorDevices.V1;

public class MonitorDeviceService
{
    private readonly IMapper _mapper;
    private readonly IDeviceRepository _deviceRepository;
    public MonitorDeviceService(IDeviceRepository deviceRepository, IMapper mapper)
    {
        _deviceRepository = deviceRepository;
        _mapper = mapper;
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


}
