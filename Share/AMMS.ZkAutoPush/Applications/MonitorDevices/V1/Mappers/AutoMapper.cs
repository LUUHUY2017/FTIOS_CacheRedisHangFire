using AMMS.ZkAutoPush.Applications.MonitorDevices.V1.Models;
using AMMS.ZkAutoPush.Datas.Entities;
using AutoMapper;

namespace AMMS.ZkAutoPush.Applications.MonitorDevices.V1;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<zk_terminal, MDeviceResponse>().ReverseMap();
    }
}
