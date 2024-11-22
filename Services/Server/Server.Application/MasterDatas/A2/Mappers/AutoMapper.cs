using AutoMapper;
using Server.Application.MasterDatas.A2.AutoReportInOut.V1.Models;
using Server.Application.MasterDatas.A2.ClassRooms.V1.Models;
using Server.Application.MasterDatas.A2.ClassRoomYears.V1.Models;
using Server.Application.MasterDatas.A2.Devices.Models;
using Server.Application.MasterDatas.A2.MonitorDevices.V1.Models;
using Server.Application.MasterDatas.A2.Organizations.V1.Models;
using Server.Application.MasterDatas.A2.ScheduleJobs.V1.Models;
using Server.Application.MasterDatas.A2.SchoolYears.V1.Models;
using Server.Application.MasterDatas.A2.Students.V1.Model;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ClassRooms.Requests;
using Server.Core.Interfaces.A2.Devices.RequeResponsessts;
using Server.Core.Interfaces.A2.ScheduleJobs.Requests;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Server.Core.Interfaces.A2.SchoolYears.Requests;
using Server.Core.Interfaces.A2.StudentClassRoomYears.Requests;
using Server.Core.Interfaces.TimeAttendenceEvents.Requests;

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

        CreateMap<ClassRoom, ClassRoomFilterRequest>().ReverseMap();
        CreateMap<ClassRoom, ClassRoomRequest>().ReverseMap();

        CreateMap<SchoolYear, SchoolYearFilterRequest>().ReverseMap();
        CreateMap<SchoolYear, SchoolYearRequest>().ReverseMap();

        CreateMap<StudentClassRoomYear, ClassRoomYearFilterRequest>().ReverseMap();
        CreateMap<StudentClassRoomYear, ClassRoomYearRequest>().ReverseMap();


        CreateMap<StudentSearchRequest, AttendenceReportFilterReq>().ReverseMap();
    }
}
