using AutoMapper;
using Server.Application.MasterDatas.A0.AttendanceConfigs.V1.Models;
using Server.Core.Entities.A0;

namespace Server.Application.MasterDatas.A0.AttendanceConfigs.V1.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<AttendanceConfig, AttendanceConfigResponse>().ReverseMap();
        CreateMap<AttendanceConfig, AttendanceConfigRequest>().ReverseMap();
    }
}
