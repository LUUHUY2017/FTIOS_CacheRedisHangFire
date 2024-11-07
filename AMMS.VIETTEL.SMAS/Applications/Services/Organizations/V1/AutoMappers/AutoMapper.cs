﻿using AMMS.VIETTEL.SMAS.Applications.Services.Organizations.V1.Models;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AutoMapper;

namespace AMMS.VIETTEL.SMAS.Applications.Services.Organizations.V1.AutoMappers;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<OrganizationRequest, A2_Organization>().ReverseMap();
        CreateMap<OrganizationResponse, A2_Organization>().ReverseMap();
    }
}
