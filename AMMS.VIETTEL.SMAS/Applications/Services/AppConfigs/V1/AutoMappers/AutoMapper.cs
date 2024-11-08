using AMMS.VIETTEL.SMAS.Applications.Services.AppConfigs.V1.Models;
using AMMS.VIETTEL.SMAS.Cores.Entities.A0;
using AutoMapper;

namespace AMMS.VIETTEL.SMAS.Applications.Services.AppConfigs.V1.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<AttendanceConfig, AppeConfigResponse>().ReverseMap();
        CreateMap<AttendanceConfig, AppConfigRequest>().ReverseMap();
    }
}
