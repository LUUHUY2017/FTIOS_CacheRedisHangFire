using AutoMapper;
using Server.Application.MasterDatas.A2.Students.V1.Model;
using Server.Core.Entities.A2;

namespace Server.Application.MasterDatas.A2.Students.V1.Mappers;

public partial class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<DtoStudentRequest, Student>().ForMember(dest => dest.ReferenceId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
        CreateMap<Student, Person>().ReverseMap();
    }
}
