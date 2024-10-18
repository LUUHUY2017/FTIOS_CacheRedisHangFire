using AutoMapper;
using Server.Application.MasterDatas.A2.Lanes.Models;
using Server.Core.Entities.A2;

namespace Server.Application.MasterDatas.A2.Lanes.AutoMappers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<LaneRequest, A2_Lane>().ReverseMap();
        }
    }
}
