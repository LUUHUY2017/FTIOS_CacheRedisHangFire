using AutoMapper;
using Server.API.APIs.Data.ScheduleSendMails.V1.Requests;
using Server.Core.Entities.A2;

namespace Server.API.APIs.Data.ScheduleSendMails.V1.Mappers;

public class AutoMappers : Profile
{
    public AutoMappers()
    {
        CreateMap<ScheduleSendMailDetail, ScheduleSendEmailDetailRequest>().ReverseMap();
    }
}
