using AMMS.VIETTEL.SMAS.Applications.Services.AppConfigs.V1.Models;
using AMMS.VIETTEL.SMAS.Applications.Services.Organizations.V1;
using AMMS.VIETTEL.SMAS.Applications.Services.Organizations.V1.Models;
using AMMS.VIETTEL.SMAS.Applications.Services.ScheduleJobs.V1.Models;
using AMMS.VIETTEL.SMAS.Applications.Services.Students.V1;
using AMMS.VIETTEL.SMAS.Applications.Services.TimeConfigs.V1.Models;
using AMMS.VIETTEL.SMAS.Cores.Entities.A0;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleJobs.Requests;
using AutoMapper;

namespace AMMS.VIETTEL.SMAS.Applications.Services.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<TimeConfig, TimeConfigRequest>().ReverseMap();
        CreateMap<TimeConfig, TimeConfigResponse>().ReverseMap();

        CreateMap<DtoStudentRequest, Student>().ForMember(dest => dest.ReferenceId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
        CreateMap<Student, Person>().ReverseMap();

        CreateMap<ScheduleJob, ScheduleJobFilterRequest>().ReverseMap();
        CreateMap<ScheduleJob, ScheduleJobRequest>().ReverseMap();

        CreateMap<OrganizationRequest, Organization>().ReverseMap();
        CreateMap<OrganizationResponse, Organization>().ReverseMap();

        CreateMap<AttendanceConfig, AppeConfigResponse>().ReverseMap();
        CreateMap<AttendanceConfig, AppConfigRequest>().ReverseMap();
    }
}
