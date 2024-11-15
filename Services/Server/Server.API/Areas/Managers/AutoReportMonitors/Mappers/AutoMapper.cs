using AutoMapper;
using Server.API.APIs.Data.ScheduleSendMails.V1.Requests;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;

namespace Server.API.Areas.Managers.AutoReportMonitors.Mappers;

public partial class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<SendEmailRequest, SendEmails>().ReverseMap();
        CreateMap<ScheduleSendEmailLogFilter, ScheduleSendEmailLogModel>().ReverseMap();
    }
}