using AutoMapper;
using Server.Application.MasterDatas.A2.Devices.Models;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Devices.RequeResponsessts;

namespace Server.Application.MasterDatas.A2.Devices.AutoMappers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<DeviceRequest, Device>().ReverseMap();
            CreateMap<DeviceResponse, Device>().ReverseMap();
        }
    }
}
