using AutoMapper;
using Server.Application.MasterDatas.A2.MonitorDevices.V1.Models;
using Server.Core.Entities.A2;

namespace Server.Application.MasterDatas.A2.MonitorDevices.V1.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<Device, MDeviceRequest>().ReverseMap();
        CreateMap<Device, MDeviceResponse>().ReverseMap();
    }
}
