using AutoMapper;
using Server.Core.Interfaces.TimeAttendenceEvents.Requests;

namespace Server.Application.MasterDatas.TA.Mappers;

public partial class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<AttendenceReportFilterReq, AttendenceSyncReportFilterReq>().ReverseMap();


    }
}
