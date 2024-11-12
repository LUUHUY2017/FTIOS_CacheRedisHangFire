using AutoMapper;
using Server.Application.MasterDatas.A0.AttendanceTimeConfigs.V1.Models;
using Server.Core.Entities.A0;

namespace Server.Application.MasterDatas.A0.AttendanceTimeConfigs.V1.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<AttendanceTimeConfig, AttendanceTimeConfigRequest>().ReverseMap();
        CreateMap<AttendanceTimeConfig, AttendanceTimeConfigResponse>().ReverseMap();
    }
}
