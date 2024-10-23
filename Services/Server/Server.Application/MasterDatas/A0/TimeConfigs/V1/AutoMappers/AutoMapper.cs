using AutoMapper;
using Server.Application.MasterDatas.A0.TimeConfigs.V1.Models;
using Server.Core.Entities.A0;

namespace Server.Application.MasterDatas.A0.TimeConfigs.V1.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<A0_TimeConfig, TimeConfigRequest>().ReverseMap();
        CreateMap<A0_TimeConfig, TimeConfigResponse>().ReverseMap();
    }
}
