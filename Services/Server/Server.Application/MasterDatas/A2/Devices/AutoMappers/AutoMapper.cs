using AutoMapper;
using Server.Application.MasterDatas.A2.Devices.Models;
using Server.Core.Entities.A2;

namespace Server.Application.MasterDatas.A2.Devices.AutoMappers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<DeviceRequest, A2_Device>().ReverseMap();
        }
    }
}
