using AMMS.DeviceData.RabbitMq;
using EventBus.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Core.Entities.A2;
using Server.Core.Entities.TA;
using Server.Core.Interfaces.A2.Persons;
using Server.Core.Interfaces.A2.Students;
using Server.Core.Interfaces.TA.TimeAttendenceSyncs;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;
using Shared.Core.Loggers;
using Shared.Core.SignalRs;
using System.Drawing;

namespace Server.Application.MasterDatas.TA.TimeAttendenceEvents.V1;
public partial class TimeAttendenceEventService
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly ISignalRClientService _signalRClientService;
    private readonly IConfiguration _configuration;

    private readonly IPersonRepository _personRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ITATimeAttendenceSyncRepository _timeAttendenceSyncRepository;

    private readonly IMasterDataDbContext _dbContext;


    public TimeAttendenceEventService(
        IBus bus,
        IConfiguration configuration,
        IEventBusAdapter eventBusAdapter,
        ISignalRClientService signalRClientService,
        IPersonRepository personRepository,
        IStudentRepository studentRepository,
        ITATimeAttendenceSyncRepository timeAttendenceSyncRepository,
        IMasterDataDbContext dbContext
        )
    {
        _configuration = configuration;
        _eventBusAdapter = eventBusAdapter;
        _signalRClientService = signalRClientService;

        _personRepository = personRepository;
        _studentRepository = studentRepository;
        _timeAttendenceSyncRepository = timeAttendenceSyncRepository;

        _dbContext = dbContext;

    }

    /// <summary>
    /// Check có tồn tại học sinh
    /// </summary>
    /// <param name="data"></param>
    /// <param name="person"></param>
    /// <param name="employee"></param>
    /// <returns></returns>
    private bool CheckStudents(TA_AttendenceHistory data, ref Student employee, ref Organization organization)
    {
        try
        {
            var employee1 = _dbContext.Student.FirstOrDefault(o => o.StudentCode == data.PersonCode);
            if (employee1 == null)
            {
                Logger.Information(data.PersonCode + "không tìm thấy thông tin học sinh");
                return false;
            }

            var organization1 = _dbContext.Organization.FirstOrDefault(o => o.Id == employee1.OrganizationId);
            if (organization1 == null)
            {
                Logger.Information(data.PersonCode + "chưa gán trường");
                return false;
            }

            employee = employee1;
            organization = organization1;

            return true;
        }
        catch (Exception e)
        {
            return false;
            Logger.Error(e);
        }

    }
    /// <summary>
    /// Xử lý điểm danh
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public async Task<string> ProcessAtendenceData(List<TA_AttendenceHistory> data)
    {
        try
        {
            //Logger.Information($"list: {JsonConvert.SerializeObject(data)} ");

            if (data == null || data.Count == 0)
                return string.Empty;

            bool add = false;
            foreach (var info in data)
            {

                Student student = null;
                Organization organization = null;
                bool isset = CheckStudents(info, ref student, ref organization);
                if (!isset)
                    continue;

                TimeSpan timeOfDay = info.TimeEvent.Value.TimeOfDay;
                DateTime eventDate = info.TimeEvent.Value.Date;
                DateTime eventDateTime = info.TimeEvent.Value;
                int hour = info.TimeEvent.Value.Hour;
                int dayOfWeek = (int)eventDate.DayOfWeek;


                var config = await _dbContext.AttendanceTimeConfig.Where(o => o.Actived == true && o.OrganizationId == student.OrganizationId
                          && timeOfDay >= o.StartTime && timeOfDay <= o.EndTime).OrderByDescending(o => o.LastModifiedDate).FirstOrDefaultAsync();
                if (config == null)
                {
                    Logger.Warning(string.Format("Học sinh:{0} - {1}:  điểm danh ngoài ca lúc {2}", student.StudentCode, student.FullName, info.TimeEvent));
                    continue;
                }


                // Parse Buổi điểm danh:0,1,2- sáng, chiều, tối
                int sectionTime = Convert.ToInt32(config.Type);

                // Nghỉ K: k phép, P: có phép, C: Có mặt, X: Đi muộn, bỏ tiết, về sớm
                string valueAttendence = "C";
                // Gửi SMS Kiểu gửi tin nhắn: 1: Gửi tin nhắn qua SMS và EduOne 2: Gửi thông báo qua EduOne 3: Gửi tin nhắn qua SMS
                int? formSendSMS = 1;
                // Đi muộn
                bool? isLate = false, isOffSoon = false;


                //if (config.BreakTime != null && timeOfDay >= config.BreakTime)
                //{
                //    valueAttendence = "K";
                //}
                //else

                if (config.LateTime != null && timeOfDay >= config.LateTime)
                {
                    isLate = true;
                    valueAttendence = "X";
                }


                bool addEvent = false;
                var startOfDay = eventDate.Date;
                var endOfDay = eventDate.Date.AddDays(1);
                // gây ra chậm hơn 
                //var time = await _dbContext.TimeAttendenceEvent.Where(o => o.EnrollNumber == info.PersonCode && o.EventTime.Value.Date == eventDate && o.AttendenceSection == sectionTime).FirstOrDefaultAsync();

                // áp dung tối ưu chỉ mục
                var time = await _dbContext.TimeAttendenceEvent.Where(o => o.OrganizationId == student.OrganizationId && o.EventTime >= startOfDay && o.EventTime < endOfDay && o.EnrollNumber == info.PersonCode && o.AttendenceSection == sectionTime).FirstOrDefaultAsync();

                if (time == null)
                {
                    time = new TimeAttendenceEvent();
                    time.Actived = true;
                    time.EventTime = info.TimeEvent;
                    addEvent = true;
                    time.IsLate = isLate;
                    time.IsLate = isOffSoon;
                    time.ValueAbSent = valueAttendence;
                }

                //time.EventType = null;
                time.InOutMode = "1";

                time.EnrollNumber = info.PersonCode;
                time.StudentCode = info.PersonCode;
                time.AttendenceSection = sectionTime;
                time.LastModifiedDate = DateTime.Now;

                time.TAMessage += timeOfDay + " | ";
                time.DeviceIP = info.SerialNumber;
                time.DeviceId = info.SerialNumber;

                time.AbsenceDate = eventDate;
                time.FormSendSMS = formSendSMS;

                time.ClassCode = student.ClassName;
                time.SchoolCode = organization.OrganizationCode;
                time.OrganizationId = organization.Id;
                time.ShiftCode = config.Id;


                if (addEvent)
                    _dbContext.TimeAttendenceEvent.Add(time);
                else
                    _dbContext.TimeAttendenceEvent.Update(time);

                await _dbContext.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return "";
    }


    /// <summary>
    /// Cập nhật trạng thái đồng bộ api điểm danh
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> SaveStatuSyncSmas(TimeAttendenceSync request)
    {
        bool statusSync = false;
        try
        {
            var data = await _timeAttendenceSyncRepository.UpdateStatusAsync(request);
            if (data.Succeeded)
            {
                statusSync = true;
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return statusSync;
    }
    /// <summary>
    /// Xử lý hình ảnh điểm danh
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public async Task<string> ProcessAttendenceImage(string base64, string id)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(base64))
            {
                var imageFolder = Common.GetImagesPathFolder("images\\attendences\\");
                var imageFullFolder = Common.GetImagesFullFolder("images\\attendences\\");

                string imageName = id + ".jpg";
                string fileName = imageFullFolder + imageName;

                Image img = Common.Base64ToImage(base64);
                if (File.Exists(fileName))
                    File.Delete(fileName);
                //img.Save(fileName);

                Common.SaveJpeg1(fileName, img, 75);
                await _timeAttendenceSyncRepository.UpdateImageAttendence(base64, id);
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return "";
    }
    /// <summary>
    /// Xử lý điểm danh - bỏ
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public async Task<string> ProcessAtendenceData1(List<TA_AttendenceHistory> data)
    {
        try
        {
            //Logger.Information($"list: {JsonConvert.SerializeObject(data)} ");

            if (data == null || data.Count == 0)
                return string.Empty;

            bool add = false;
            foreach (var info in data)
            {

                Student student = null;
                Organization organization = null;
                bool isset = CheckStudents(info, ref student, ref organization);
                if (!isset)
                    continue;

                TimeSpan timeOfDay = info.TimeEvent.Value.TimeOfDay;
                DateTime eventDate = info.TimeEvent.Value.Date;
                DateTime eventDateTime = info.TimeEvent.Value;
                int hour = info.TimeEvent.Value.Hour;
                int dayOfWeek = (int)eventDate.DayOfWeek;


                var config = await _dbContext.AttendanceTimeConfig.Where(o => o.Actived == true && o.OrganizationId == student.OrganizationId
                          && timeOfDay >= o.StartTime && timeOfDay <= o.EndTime).OrderByDescending(o => o.LastModifiedDate).FirstOrDefaultAsync();
                if (config == null)
                {
                    Logger.Warning(string.Format("Học sinh:{0} - {1}:  điểm danh ngoài ca lúc {2}", student.StudentCode, student.FullName, info.TimeEvent));
                    continue;
                }


                // Parse Buổi điểm danh:0,1,2- sáng, chiều, tối
                int sectionTime = Convert.ToInt32(config.Type);

                // Nghỉ K: k phép, P: có phép, C: Có mặt, X: Đi muộn, bỏ tiết, về sớm
                string valueAttendence = "C";
                // Gửi SMS Kiểu gửi tin nhắn: 1: Gửi tin nhắn qua SMS và EduOne 2: Gửi thông báo qua EduOne 3: Gửi tin nhắn qua SMS
                int? formSendSMS = 1;
                // Đi muộn
                bool? isLate = null, isOffSoon = null;


                //if (config.BreakTime != null && timeOfDay >= config.BreakTime)
                //{
                //    valueAttendence = "K";
                //}
                //else

                if (config.LateTime != null && timeOfDay >= config.LateTime)
                {
                    isLate = true;
                    valueAttendence = "X";
                }


                bool addEvent = false;
                var startOfDay = eventDate.Date;
                var endOfDay = eventDate.Date.AddDays(1);
                // gây ra chậm hơn 
                //var time = await _dbContext.TimeAttendenceEvent.Where(o => o.EnrollNumber == info.PersonCode && o.EventTime.Value.Date == eventDate && o.AttendenceSection == sectionTime).FirstOrDefaultAsync();

                // áp dung tối ưu chỉ mục
                var time = await _dbContext.TimeAttendenceEvent.Where(o => o.OrganizationId == student.OrganizationId && o.EventTime >= startOfDay && o.EventTime < endOfDay && o.EnrollNumber == info.PersonCode && o.AttendenceSection == sectionTime).FirstOrDefaultAsync();

                if (time == null)
                {
                    time = new TimeAttendenceEvent();
                    time.Actived = true;
                    time.EventTime = info.TimeEvent;
                    addEvent = true;
                    time.IsLate = isLate;
                    time.ValueAbSent = valueAttendence;
                }

                time.EventType = null;
                time.InOutMode = "1";

                time.EnrollNumber = info.PersonCode;
                time.StudentCode = info.PersonCode;
                time.AttendenceSection = sectionTime;
                time.LastModifiedDate = DateTime.Now;

                time.TAMessage = "Success";
                time.DeviceIP = info.SerialNumber;
                time.DeviceId = info.SerialNumber;

                time.AbsenceDate = eventDate;
                time.FormSendSMS = formSendSMS;

                time.ClassCode = student.ClassName;
                time.SchoolCode = organization.OrganizationCode;
                time.OrganizationId = organization.Id;
                time.ShiftCode = config.Id;


                if (addEvent)
                    _dbContext.TimeAttendenceEvent.Add(time);
                else
                    _dbContext.TimeAttendenceEvent.Update(time);

                if (valueAttendence == "X" && valueAttendence == "C")
                {
                    var history = _dbContext.TimeAttendenceDetail.FirstOrDefault(o => o.TA_TimeAttendenceEventId == time.Id);
                    if (history == null)
                    {
                        history = new TimeAttendenceDetail();
                        history.Actived = true;
                        history.TA_TimeAttendenceEventId = time.Id;
                        add = true;
                    }

                    history.IsLate = isLate;
                    history.IsOffSoon = isOffSoon;
                    history.PeriodI = false;
                    history.PeriodII = false;
                    history.PeriodIII = false;
                    history.PeriodIV = false;
                    history.PeriodIV = false;
                    history.PeriodVI = false;
                    history.AbsenceTime = info.TimeEvent;

                    if (add)
                        _dbContext.TimeAttendenceDetail.Add(history);
                    else
                        _dbContext.TimeAttendenceDetail.Update(history);
                }

                await _dbContext.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return "";
    }
}

