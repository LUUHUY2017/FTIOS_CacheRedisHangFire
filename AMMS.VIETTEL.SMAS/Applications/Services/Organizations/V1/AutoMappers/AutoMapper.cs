using AMMS.VIETTEL.SMAS.Applications.Services.Organizations.V1.Models;
using AMMS.VIETTEL.SMAS.Cores.Entities;
using AutoMapper;

namespace AMMS.VIETTEL.SMAS.Applications.Services.Organizations.V1.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<OrganizationRequest, Organization>().ReverseMap();
        CreateMap<OrganizationResponse, Organization>().ReverseMap();
    }
}
