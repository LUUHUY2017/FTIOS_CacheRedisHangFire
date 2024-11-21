using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Loggers;

namespace AMMS.VIETTEL.SMAS.Applications.CronJobs;

public partial class CronJobService : ICronJobService
{
    public async Task<string> CheckTimeSyncAttendence(string orgId)
    {
        string aboutHour = "*";
        try
        {
            var minMaxTimes = await _dbContext.AttendanceTimeConfig
                              .Where(o => o.OrganizationId == orgId && o.Actived == true).Select(o => new
                              {
                                  MinStartTime = o.StartTime,
                                  MaxEndTime = o.EndTime
                              })
                              .Where(o => o.MinStartTime.HasValue && o.MaxEndTime.HasValue)
                              .ToListAsync();
            if (minMaxTimes.Any())
            {
                var minStartTime = minMaxTimes.Min(o => o.MinStartTime.Value.Hours);
                var maxEndTime = minMaxTimes.Max(o => o.MaxEndTime.Value.Hours);

                string startRun = minStartTime > 0 ? (minStartTime).ToString() : "0";
                //string endRun = maxEndTime < 23 ? (maxEndTime + 1).ToString() : "23";
                string endRun = "22";
                aboutHour = $"{startRun}-{endRun}";
            }
        }
        catch (Exception ex) { }
        return aboutHour;
    }
    public async Task CreateScheduleCronJob(List<ScheduleJob> scheduleLists)
    {
        string aboutHour = "*";

        foreach (var item in scheduleLists)
        {
            var timeSentHour = item.ScheduleTime.HasValue ? item.ScheduleTime.Value.Hours : 0;
            var timeSentMinute = item.ScheduleTime.HasValue ? item.ScheduleTime.Value.Minutes : 0;


            if (item.ScheduleNote == "LAPLICHDONGBO")
            {
                if (item.ScheduleType == "DONGBODIEMDANH")
                    aboutHour = await CheckTimeSyncAttendence(item.OrganizationId);

                var newCronExpression = item.ScheduleSequential switch
                {
                    // giây phút giờ ngày tháng 1-7(thứ 2-CN)
                    //Lưu ý:  ScronJob chạy từng giây: thường k support
                    "5s" => $"*/5 * {aboutHour} * * *",
                    "10s" => $"*/10 * {aboutHour} * * *",
                    "20s" => $"*/20 * {aboutHour} * * *",
                    "30s" => $"*/30 * {aboutHour} * * *",
                    "40s" => $"*/40 * {aboutHour} * * *",
                    "50s" => $"*/50 * {aboutHour} * * *",

                    "Minutely" => $"* {aboutHour} * * *",
                    "2M" => $"*/2 {aboutHour} * * *",
                    "3M" => $"*/3 {aboutHour} * * *",
                    "4M" => $"*/4 {aboutHour} * * *",
                    "5M" => $"*/5 {aboutHour} * * *",
                    "10M" => $"*/10 {aboutHour} * * *",
                    "15M" => $"*/15 {aboutHour} * * *",
                    "20M" => $"*/20 {aboutHour} * * *",
                    "30M" => "*/30 * * * *",
                    "40M" => "*/40 * * * *",
                    "50M" => "*/50 * * * *",

                    "Hourly" => "0 * * * *",
                    "Daily" => $"{timeSentMinute} {timeSentHour} * * *",
                    "Weekly" => $"{timeSentMinute} {timeSentHour} * * 0",
                    "Monthly" => $"{timeSentMinute} {timeSentHour} 1 * *",
                    "Yearly" => $"{timeSentMinute} {timeSentHour} 1 1 *",
                    _ => throw new ArgumentException("Invalid ScheduleSequential value")
                };

                if (item.ScheduleType == "DONGBOHOCSINH")
                    await UpdateScheduleSyncStudentCronJob("CronJobSyncSmas[*]" + item.ScheduleType, item.Id, newCronExpression);
                if (item.ScheduleType == "DONGBODIEMDANH")
                    await UpdateScheduleSyncAttendenceCronJob("CronJobSyncSmas[*]" + item.ScheduleType, item.Id, newCronExpression);

                //RecurringJob.RemoveIfExists("CronJobSyncSmas[*]" + item.ScheduleType + "_" + item.Id);
            }
        }
    }

    public async Task UpdateScheduleSyncStudentCronJob(string jobId, string sheduleId, string newCronExpression)
    {
        string JobName = jobId + "_" + sheduleId;
        RecurringJob.AddOrUpdate(JobName, () => SyncStudentFromSmas(sheduleId), newCronExpression, TimeZoneInfo.Local);
    }
    public async Task UpdateScheduleSyncAttendenceCronJob(string jobId, string sheduleId, string newCronExpression)
    {
        string JobName = jobId + "_" + sheduleId;
        RecurringJob.AddOrUpdate(JobName, () => SyncAttendenceToSmas(sheduleId), newCronExpression, TimeZoneInfo.Local);
    }
    public async Task RemoveScheduleCronJob(string jobId, string sheduleId)
    {
        string JobName = jobId + "_" + sheduleId;
        RecurringJob.RemoveIfExists(JobName);
    }

    public async Task SyncStudentFromSmas(string sheduleId)
    {
        DateTime now = DateTime.Now;
        var SyncSmas_Image = _configuration.GetValue<string>("SyncSmas:Image");
        var SyncSmas_Class = _configuration.GetValue<string>("SyncSmas:Class");

        try
        {
            var jobRes = await _dbContext.ScheduleJob.FirstOrDefaultAsync(o => o.Actived == true && o.Id == sheduleId);
            if (jobRes == null)
                return;

            var orgRes = await _dbContext.Organization.FirstOrDefaultAsync(o => o.Id == jobRes.OrganizationId && o.Actived == true);
            if (orgRes == null)
                return;

            string schoolCode = orgRes.OrganizationCode; // "20186511"
            var res = await _smartService.PostListStudents(schoolCode);

            var logSchedule = new ScheduleJobLog()
            {
                Actived = true,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                OrganizationId = orgRes.Id,
                Logs = jobRes.ScheduleJobName,
                ScheduleJobId = jobRes.Id,
            };

            int count = 0, i = 0;
            if (res.Any())
            {
                count = res.Count();
                foreach (var item in res)
                {
                    i = i + 1;
                    string lastName = !string.IsNullOrWhiteSpace(item.StudentName) ? item.StudentName.Trim().Split(' ').Last() : "";

                    var el = new Student()
                    {
                        SyncCode = item.SyncCode,
                        StudentCode = item.StudentCode,
                        EthnicCode = item.StudentCode?.Replace("-", "").Replace(" ", ""),

                        ClassId = item.ClassId,
                        ClassName = item.ClassName,
                        DateOfBirth = item.BirthDay,
                        FullName = item.StudentName,
                        Name = lastName,
                        OrganizationId = orgRes.Id,
                        SchoolCode = orgRes.OrganizationCode,
                        ImageSrc = item.ImgSrc,
                    };

                    // Lưu thông tin lớp
                    try
                    {
                        if (SyncSmas_Class != null && SyncSmas_Class == "True")
                        {
                            var resQ = await _schoolYearClassService.SaveFromService(el);
                            if (resQ.Succeeded)
                                el.StudentClassId = resQ.Data.Id;
                        }

                    }
                    catch (Exception ex) { Logger.Warning(ex.Message); }

                    // Lưu thông tin học sinh
                    var resS = await _studentService.SaveFromService(el);

                    /// Lưu ảnh học sinh
                    try
                    {
                        if (SyncSmas_Image != null && SyncSmas_Image == "True")
                        {
                            if (resS.Succeeded && !string.IsNullOrWhiteSpace(el.ImageSrc))
                            {
                                var stu = resS.Data;
                                stu.ImageSrc = el.ImageSrc;
                                await _studentService.PushImageToRabbit(stu);
                            }
                        }
                    }
                    catch (Exception ex) { Logger.Warning(ex.Message); }


                }

                logSchedule.ScheduleJobStatus = true;
                logSchedule.ScheduleLogNote = "Thành công";
                logSchedule.Message = string.Format("Đã đồng bộ {0}/{1} học sinh từ SMAS", i, count);
            }
            else
            {
                logSchedule.ScheduleJobStatus = false;
                logSchedule.ScheduleLogNote = "Thành công";
                logSchedule.Message = string.Format("Không có bản tin nào trả về");
            }


            await _dbContext.ScheduleJobLog.AddAsync(logSchedule);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Logger.Warning(ex.Message);
        }
    }



}
