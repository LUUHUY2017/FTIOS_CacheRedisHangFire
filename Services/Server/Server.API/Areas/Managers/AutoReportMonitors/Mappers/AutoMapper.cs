using AutoMapper;
using Server.API.APIs.Data.ScheduleSendMails.V1.Requests;

//using Server.API.APIs.Data.ScheduleSendMails.V1.Requests;
using Server.Core.Entities.A2;

namespace Server.API.Areas.Managers.AutoReportMonitors.Mappers;

public partial class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<SendEmailRequest, A2_SendEmail>().ReverseMap();
    }
}