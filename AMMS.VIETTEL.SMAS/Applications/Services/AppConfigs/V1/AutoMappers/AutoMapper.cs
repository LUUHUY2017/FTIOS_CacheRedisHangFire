using AMMS.VIETTEL.SMAS.Applications.Services.AppConfigs.V1.Models;
using AMMS.VIETTEL.SMAS.Cores.Entities.A0;
using AutoMapper;

namespace AMMS.VIETTEL.SMAS.Applications.Services.AppConfigs.V1.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<A0_AttendanceConfig, AppeConfigResponse>().ReverseMap();
        CreateMap<A0_AttendanceConfig, AppConfigRequest>().ReverseMap();
    }
}
