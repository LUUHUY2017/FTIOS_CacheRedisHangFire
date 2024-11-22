using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart.Responses;
using AMMS.VIETTEL.SMAS.Cores.Entities.TA;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Core.Loggers;

namespace AMMS.VIETTEL.SMAS.Applications.CronJobs;

public partial class CronJobService : ICronJobService
{

    public async Task SyncAttendenceToSmas(string sheduleId)
    {
        DateTime now = DateTime.Now;
        try
        {
            var jobRes = await _dbContext.ScheduleJob.FirstOrDefaultAsync(o => o.Actived == true && o.Id == sheduleId);
            if (jobRes == null)
                return;

            var orgRes = await _dbContext.Organization.FirstOrDefaultAsync(o => o.Id == jobRes.OrganizationId && o.Actived == true);
            if (orgRes == null)
                return;

            // Lấy dữ liệu theo block gửi qua api
            var datas = await _dbContext.TimeAttendenceEvent.Where(o => o.OrganizationId == orgRes.Id && o.EventType != true).OrderBy(o => o.EventTime).Take(50).ToListAsync();
            if (datas.Count == 0)
                return;

            var studentAbs = new List<StudentAbsence>();
            foreach (var item in datas)
            {
                ExtraProperties extra = new ExtraProperties()
                {
                    isLate = item.IsLate != null ? item.IsLate : false,
                    lateTime = item.IsLate == true ? item.EventTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                    absenceTime = item.EventTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                };
                var el = new StudentAbsence()
                {
                    studentCode = item.StudentCode,
                    value = item.ValueAbSent,
                    extraProperties = extra
                };
                studentAbs.Add(el);
            }
            var req = new SyncDataRequest()
            {
                id = Guid.NewGuid().ToString(),
                schoolCode = orgRes.OrganizationCode,
                absenceDate = DateTime.Now,
                section = 0,
                formSendSMS = 1,
                studentCodeType = 2,
                studentAbsenceByDevices = studentAbs,
            };

            //Logger.Warning("SMAS_Req:" + JsonConvert.SerializeObject(req));
            var res = await _smartService.PostSyncAttendence2Smas(req, orgRes.OrganizationCode);
            Logger.Warning("SMAS_Res:" + orgRes.OrganizationCode + JsonConvert.SerializeObject(res));

            if (res == null || !res.IsSuccess)
                return;

            datas.ForEach(o => { o.EventType = true; });

            try
            {
                var _listLog = new List<TimeAttendenceSync>();
                foreach (var item in res.Responses)
                {
                    var el = datas.FirstOrDefault(o => o.StudentCode == item.studentCode && o.EventTime.Value.ToString("yyyy-MM-dd HH:mm:ss") == item.extraProperties.absenceTime);
                    if (el == null)
                        continue;

                    var log = new TimeAttendenceSync()
                    {
                        TimeAttendenceEventId = el.Id,
                        SyncStatus = item.status,
                        Message = $"[{DateTime.Now:dd/MM/yy HH:mm:ss}]: {item.message}\r\n",
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    };
                    _listLog.Add(log);
                }

                await _dbContext.TimeAttendenceSync.AddRangeAsync(_listLog);
            }
            catch (Exception ext)
            {
                Logger.Error(ext);
            }
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }
}
