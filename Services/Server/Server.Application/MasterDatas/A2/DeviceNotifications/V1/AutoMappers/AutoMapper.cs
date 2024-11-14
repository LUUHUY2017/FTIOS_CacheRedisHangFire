using AutoMapper;
using Server.Application.MasterDatas.A2.DeviceNotifications.V1.Models;
using Server.Core.Entities.A2;

namespace Server.Application.MasterDatas.A2.DeviceNotifications.V1
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<DeviceNotificationResponse, ScheduleSendMail>().ReverseMap();
            CreateMap<DeviceNotificationRequest, ScheduleSendMail>().ReverseMap();
        }
    }
}
