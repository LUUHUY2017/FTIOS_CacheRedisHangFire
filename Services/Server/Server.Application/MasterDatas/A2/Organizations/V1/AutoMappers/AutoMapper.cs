using AutoMapper;
using Server.Application.MasterDatas.A2.Organizations.V1.Models;
using Server.Core.Entities.A2;

namespace Server.Application.MasterDatas.A2.Organizations.V1.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<OrganizationRequest, Organization>().ReverseMap();
        CreateMap<OrganizationResponse, Organization>().ReverseMap();
    }
}
