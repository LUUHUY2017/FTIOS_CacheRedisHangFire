using AutoMapper;
using Server.API.APIs.Data.ScheduleSendMails.V1.Requests;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;

namespace Server.API.Areas.Managers.AutoReports.Mappers;

public partial class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<A2_ScheduleSendMail, ScheduleSendEmailRequest>().ReverseMap();
        CreateMap<A2_ScheduleSendMailDetail, ScheduleSendEmailDetailRequest>().ReverseMap();
        CreateMap<ScheduleSendEmailFilter, ScheduleSendEmailModel>().ReverseMap();
        CreateMap<ScheduleSendEmailLogFilter, ScheduleSendEmailLogModel>().ReverseMap();



    }
}