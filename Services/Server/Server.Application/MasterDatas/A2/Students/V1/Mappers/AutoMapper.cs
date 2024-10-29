using AutoMapper;
using Server.Application.MasterDatas.A2.Students.V1.Model;
using Server.Core.Entities.A2;

namespace Server.Application.MasterDatas.A2.Students.V1.Mappers;

public partial class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<DtoStudentRequest, A2_Student>().ReverseMap();
        CreateMap<A2_Student, A2_Person>().ReverseMap();
    }
}
