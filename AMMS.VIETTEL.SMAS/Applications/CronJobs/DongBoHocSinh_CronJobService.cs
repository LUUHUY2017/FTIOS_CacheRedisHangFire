using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Loggers;

namespace AMMS.VIETTEL.SMAS.Applications.CronJobs;

public partial class CronJobService : ICronJobService
{
    public async Task CreateScheduleCronJob(List<ScheduleJob> scheduleLists)
    {
        foreach (var item in scheduleLists)
        {
            var timeSentHour = item.ScheduleTime.HasValue ? item.ScheduleTime.Value.Hours : 0;
            var timeSentMinute = item.ScheduleTime.HasValue ? item.ScheduleTime.Value.Minutes : 0;
            if (item.ScheduleNote == "LAPLICHDONGBO")
            {
                var newCronExpression = item.ScheduleSequential switch
                {
                    "5s" => "*/5 * * * * *",
                    "10s" => "*/10 * * * * *",
                    "20s" => "*/20 * * * * *",
                    "30s" => "*/30 * * * * *",
                    "40s" => "*/40 * * * * *",
                    "50s" => "*/50 * * * * *",

                    "Minutely" => "* * * * *",
                    "5M" => "*/5 * * * *",
                    "10M" => "*/10 * * * *",
                    "20M" => "*/20 * * * *",
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
