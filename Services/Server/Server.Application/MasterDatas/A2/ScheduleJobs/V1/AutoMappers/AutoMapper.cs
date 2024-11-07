using AutoMapper;
using Server.Application.MasterDatas.A2.AutoReportInOut.V1.Models;
using Server.Application.MasterDatas.A2.ScheduleJobs.V1.Models;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleJobs.Requests;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;

namespace Server.Application.MasterDatas.A2.ScheduleJobs.V1.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {

        CreateMap<ScheduleJob, ScheduleJobFilterRequest>().ReverseMap();
        CreateMap<ScheduleJob, ScheduleJobRequest>().ReverseMap();
    }
}
