using AMMS.VIETTEL.SMAS.Applications.Services.ScheduleJobs.V1.Models;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleJobs.Requests;
using AutoMapper;

namespace AMMS.VIETTEL.SMAS.Applications.Services.ScheduleJobs.V1.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {

        CreateMap<ScheduleJob, ScheduleJobFilterRequest>().ReverseMap();
        CreateMap<ScheduleJob, ScheduleJobRequest>().ReverseMap();
    }
}
