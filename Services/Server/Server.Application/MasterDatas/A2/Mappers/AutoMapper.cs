using AutoMapper;
using Server.Application.MasterDatas.A2.AutoReportInOut.V1.Models;
using Server.Application.MasterDatas.A2.Devices.Models;
using Server.Application.MasterDatas.A2.MonitorDevices.V1.Models;
using Server.Application.MasterDatas.A2.Organizations.V1.Models;
using Server.Application.MasterDatas.A2.ScheduleJobs.V1.Models;
using Server.Application.MasterDatas.A2.Students.V1.Model;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Devices.RequeResponsessts;
using Server.Core.Interfaces.A2.ScheduleJobs.Requests;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;

namespace Server.Application.MasterDatas.A2.Mappers;

public partial class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<DtoStudentRequest, Student>().ForMember(dest => dest.ReferenceId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
        CreateMap<Student, Person>().ReverseMap();
        CreateMap<Device, MDeviceRequest>().ReverseMap();
        CreateMap<Device, MDeviceResponse>().ReverseMap();
        CreateMap<OrganizationRequest, Organization>().ReverseMap();
        CreateMap<OrganizationResponse, Organization>().ReverseMap();
        CreateMap<ScheduleJob, ScheduleJobFilterRequest>().ReverseMap();
        CreateMap<ScheduleJob, ScheduleJobRequest>().ReverseMap();
        CreateMap<DeviceRequest, Device>().ReverseMap();
        CreateMap<DeviceResponse, Device>().ReverseMap();
        CreateMap<ScheduleReportInOutFilter, ScheduleSendEmailFilterRequest>().ReverseMap();
        CreateMap<ScheduleSendMail, ScheduleReportInOutRequest>().ReverseMap();
        CreateMap<ScheduleSendMail, ScheduleReportInOutResponse>().ReverseMap();
    }
}
