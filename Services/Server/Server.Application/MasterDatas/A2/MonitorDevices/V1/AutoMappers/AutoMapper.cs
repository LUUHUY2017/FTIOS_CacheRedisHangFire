using AutoMapper;
using Server.Application.MasterDatas.A2.MonitorDevices.V1.Models;
using Server.Core.Entities.A2;

namespace Server.Application.MasterDatas.A2.MonitorDevices.V1.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<A2_Device, MDeviceRequest>().ReverseMap();
        CreateMap<A2_Device, MDeviceResponse>().ReverseMap();
    }
}
