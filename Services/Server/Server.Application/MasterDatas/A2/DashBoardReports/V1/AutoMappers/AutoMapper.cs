using AutoMapper;
using Server.Application.MasterDatas.A2.DashBoardReports.V1.Models;
using Server.Core.Entities.A2;

namespace Server.Application.MasterDatas.A2.DashBoardReports.V1.AutoMappers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<DashBoardReportRequest, ScheduleSendMail>().ReverseMap();
            CreateMap<DashBoardReportRequestResponse, ScheduleSendMail>().ReverseMap();
        }
    }
}
