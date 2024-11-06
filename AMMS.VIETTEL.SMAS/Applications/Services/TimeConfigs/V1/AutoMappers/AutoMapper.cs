using AMMS.VIETTEL.SMAS.Applications.Services.TimeConfigs.V1.Models;
using AMMS.VIETTEL.SMAS.Cores.Entities;
using AutoMapper;

namespace AMMS.VIETTEL.SMAS.Applications.Services.TimeConfigs.V1.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<TimeConfig, TimeConfigRequest>().ReverseMap();
        CreateMap<TimeConfig, TimeConfigResponse>().ReverseMap();
    }
}
