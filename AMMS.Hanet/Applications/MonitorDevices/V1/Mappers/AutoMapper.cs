using AMMS.Hanet.Applications.MonitorDevices.V1.Models;
using AMMS.Hanet.Datas.Entities;
using AutoMapper;

namespace AMMS.Hanet.Applications.MonitorDevices.V1.Mappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<hanet_terminal, MDeviceResponse>().ReverseMap();
    }
}
