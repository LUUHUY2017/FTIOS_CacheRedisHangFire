using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AutoMapper;

namespace AMMS.VIETTEL.SMAS.Applications.Services.Students.V1.Mappers;

public partial class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<DtoStudentRequest, Student>().ForMember(dest => dest.ReferenceId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
        CreateMap<Student, Person>().ReverseMap();
    }
}
