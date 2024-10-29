using AMMS.Hanet.Applications.AppConfigs.V1.Models;
using AMMS.Hanet.Datas.Entities;
using AutoMapper;

namespace AMMS.Hanet.Applications.AppConfigs.V1.Mappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<app_config, AppConfigRequest>().ReverseMap();
        CreateMap<app_config, AppConfigResponse>().ReverseMap();
    }
}
