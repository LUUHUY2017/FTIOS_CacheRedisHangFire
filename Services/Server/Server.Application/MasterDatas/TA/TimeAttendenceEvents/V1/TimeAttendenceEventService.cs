using AMMS.DeviceData.RabbitMq;
using EventBus.Messages;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Server.Application.Services.VTSmart.Responses;
using Server.Core.Entities.A2;
using Server.Core.Entities.TA;
using Server.Core.Interfaces.A2.Persons;
using Server.Core.Interfaces.A2.Students;
using Server.Core.Interfaces.TA.TimeAttendenceSyncs;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;
using Shared.Core.Loggers;
using Shared.Core.SignalRs;

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
    /// Gửi qua RabbitMQ đồng bộ diểm danh
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public async Task<Result<SyncDataRequest>> PushAttendence2SMAS(SyncDataRequest data)
    {
        try
        {
            var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Server_Auto_Push_SMAS}");
            await aa.Send(data);

            return new Result<SyncDataRequest>($"Gửi thành công", true);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return new Result<SyncDataRequest>($"Gửi email lỗi: {e.Message}", false);
        }
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

            if (data.Any())
            {
                bool addEvent = false;
                bool add = false;

                foreach (var info in data)
                {

                    Student student = null;
                    Organization organization = null;
                    bool isset = CheckStudents(info, ref student, ref organization);
                    if (!isset)
                        continue;


                    var config = _dbContext.TimeConfig.Where(o => o.Actived == true && o.OrganizationId == student.OrganizationId).FirstOrDefault();

                    if (config != null)
                    {
                        TimeSpan timeOfDay = info.TimeEvent.Value.TimeOfDay;
                        DateTime date = info.TimeEvent.Value.Date;
                        DateTime dateTime = info.TimeEvent.Value;

                        int hour = info.TimeEvent.Value.Hour;
                        int dayOfWeek = (int)dateTime.DayOfWeek;

                        // Buổi sáng
                        int sectionTime = 0;
                        //// Đi muộn
                        //bool isLate = false;

                        //// Về sớm
                        //bool isOffSoon = false;
                        //// Bỏ tiết
                        //bool isOffPeriod = false;
                        //// Đi muộn
                        //DateTime? lateTime = null;
                        //DateTime? offSoonTime = null;

                        // Nghỉ K: k phép, P: có phép, C: Có mặt, X: Đi muộn, bỏ tiết, về sớm
                        string valueAttendence = "C";

                        // Gửi SMS Kiểu gửi tin nhắn: 1: Gửi tin nhắn qua SMS và EduOne 2: Gửi thông báo qua EduOne 3: Gửi tin nhắn qua SMS
                        int? formSendSMS = null;

                        //if (hour >= 12)
                        //{
                        //    sectionTime = 1;
                        //}
                        //if (hour >= 18)
                        //{
                        //    sectionTime = 2;
                        //}

                        ///// Buổi sáng
                        //if (sectionTime == 0 && timeOfDay >= config.MorningBreakTime)
                        //{
                        //    valueAttendence = "K";
                        //    formSendSMS = 3;
                        //}

                        //if (sectionTime == 0 && timeOfDay >= config.MorningLateTime && timeOfDay < config.MorningBreakTime)
                        //{
                        //    valueAttendence = "X";
                        //    isLate = true;
                        //    formSendSMS = 3;
                        //}

                        //// Buổi chiều
                        //if (sectionTime == 1 && timeOfDay >= config.AfternoonBreakTime)
                        //{
                        //    valueAttendence = "K";
                        //    formSendSMS = 3;
                        //}

                        //if (sectionTime == 1 && timeOfDay >= config.AfternoonLateTime && timeOfDay < config.AfternoonBreakTime)
                        //{
                        //    valueAttendence = "X";
                        //    isLate = true;
                        //    formSendSMS = 3;
                        //}

                        if (timeOfDay >= config.MorningStartTime && timeOfDay < config.MorningEndTime)
                        {
                            sectionTime = 0;
                            valueAttendence = "C";
                        }

                        else if (timeOfDay >= config.AfternoonStartTime && timeOfDay < config.AfternoonEndTime)
                        {
                            sectionTime = 1;
                            valueAttendence = "C";
                        }

                        else if (timeOfDay >= config.EveningStartTime && timeOfDay < config.EveningEndTime)
                        {
                            sectionTime = 2;
                            valueAttendence = "C";
                        }

                        var time = _dbContext.TimeAttendenceEvent.Where(o => o.EnrollNumber == info.PersonCode
                        && o.EventTime.Value.Date == info.TimeEvent.Value.Date && o.AttendenceSection == sectionTime
                        ).FirstOrDefault();

                        if (time == null)
                        {
                            time = new TimeAttendenceEvent();
                            time.Actived = true;
                            addEvent = true;
                        }

                        time.EventTime = info.TimeEvent;
                        time.EnrollNumber = info.PersonCode;
                        time.StudentCode = info.PersonCode;

                        time.TAMessage = "Success";
                        time.DeviceIP = info.SerialNumber;
                        time.DeviceId = info.SerialNumber;

                        time.FormSendSMS = formSendSMS;
                        time.AbsenceDate = date;
                        time.ValueAbSent = valueAttendence;
                        time.AttendenceSection = sectionTime;

                        time.SchoolCode = organization.OrganizationCode;
                        time.OrganizationId = organization.Id;


                        if (addEvent)
                            _dbContext.TimeAttendenceEvent.Add(time);
                        else
                            _dbContext.TimeAttendenceEvent.Update(time);
                    }
                    await _dbContext.SaveChangesAsync();
                }
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
            if (base64 != null)
            {
                await _timeAttendenceSyncRepository.UpdateImageAttendence(base64, id);
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return "";
    }

}

