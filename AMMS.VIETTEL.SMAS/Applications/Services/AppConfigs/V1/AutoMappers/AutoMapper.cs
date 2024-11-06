using AMMS.VIETTEL.SMAS.Applications.Services.AppConfigs.V1.Models;
using AMMS.VIETTEL.SMAS.Cores.Entities;
using AutoMapper;

namespace AMMS.VIETTEL.SMAS.Applications.Services.AppConfigs.V1.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<app_config, AppeConfigResponse>().ReverseMap();
        CreateMap<app_config, AppConfigRequest>().ReverseMap();
    }
}
