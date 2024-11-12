using AutoMapper;
using Server.Application.MasterDatas.A0.RollCallTimeConfigs.V1.Models;
using Server.Core.Entities.A0;

namespace Server.Application.MasterDatas.A0.RollCallTimeConfigs.V1.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<RollCallTimeConfig, RollCallTimeConfigRequest>().ReverseMap();
        CreateMap<RollCallTimeConfig, RollCallTimeConfigResponse>().ReverseMap();
    }
}
