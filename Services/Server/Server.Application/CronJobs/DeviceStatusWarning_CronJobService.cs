using AMMS.Notification.Commons;
using ClosedXML.Report;
using Hangfire;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using Server.Application.CronJobs.Reponses;
using Server.Application.MasterDatas.A2.DeviceNotifications.V1.Models.Reports;
using Server.Core.Entities.A0;
using Server.Core.Entities.A2;
using Shared.Core.Commons;
using Shared.Core.Emails.V1.Commons;
using Shared.Core.Loggers;

namespace Server.Application.CronJobs;

public partial class CronJobService : ICronJobService
{
    #region Cảnh báo thiết bị
    static bool Is_CheckDeviceStatusWarning { get; set; } = false;
    public async Task CreateDeviceStatusWarningCronJob(List<ScheduleSendMail> scheduleLists)
    {
        foreach (var item in scheduleLists)
        {
            var timeSentHour = item.ScheduleTimeSend.HasValue ? item.ScheduleTimeSend.Value.Hours : 0;
            var timeSentMinute = item.ScheduleTimeSend.HasValue ? item.ScheduleTimeSend.Value.Hours : 0;
            var newCronExpression = "0 * * * *";

            //////////////////////////////////////??>>>>>
            if (item.ScheduleNote == NotificationConst.CANHBAOTHIETBIMATKETNOI)
            {
                if (item.ScheduleSequentialSending == "Hourly")
                {
                    timeSentHour = item.ScheduleTimeStart.Value.Hours;
                    var endSentHour = item.ScheduleTimeEnd.Value.Hours;
                    //newCronExpression = $" */5 {timeSentHour}-{endSentHour} * * *";
                    newCronExpression = $" 0 {timeSentHour}-{endSentHour} * * *";
                    UpdateDeviceStatusWarningCronJob(NotificationConst.CANHBAOTHIETBIMATKETNOI, item.Id, newCronExpression);
                }
                if (item.ScheduleSequentialSending == "TwoHourly")
                {
                    timeSentHour = item.ScheduleTimeStart.Value.Hours;
                    var endSentHour = item.ScheduleTimeEnd.Value.Hours;
                    newCronExpression = $"0 {timeSentHour}-{endSentHour}/{2} * * *";
                    UpdateDeviceStatusWarningCronJob(NotificationConst.CANHBAOTHIETBIMATKETNOI, item.Id, newCronExpression);
                }
            }

        }
    }

    public async Task UpdateDeviceStatusWarningCronJob(string jobId, string sheduleId, string newCronExpression)
    {
        var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
        string JobName = jobId + "_" + sheduleId;

        if (jobId == NotificationConst.CANHBAOTHIETBIMATKETNOI)
        {
            RecurringJob.AddOrUpdate(JobName, () => Device_Warning_ScheduleSendMail(sheduleId), newCronExpression, TimeZoneInfo.Local);
        }

    }
    public async Task Device_Warning_ScheduleSendMail(string sheduleId)
    {
        //if (!Is_Run_POC_Report_ScheduleSendMailReportDaily)
        //{
        //    Is_Run_POC_Report_ScheduleSendMailReportDaily = true;
        ScheduleSendMailDeviceWarning(sheduleId).Wait();
        //}
    }
    public async Task ScheduleSendMailDeviceWarning(string sheduleId)
    {
        if (Is_CheckDeviceStatusWarning)
            return;

        Is_CheckDeviceStatusWarning = true;

        DateTime now = DateTime.Now;
        try
        {
            var datas = await _dbContext.ScheduleSendMail.Where(o => o.Actived == true && o.Id == sheduleId).ToListAsync();
            if (datas.Any())
            {
                foreach (var item in datas)
                {
                    DateTime date = DateTime.Now;
                    if (sheduleId != item.Id)
                        continue;

                    var users = await _dbContext.ScheduleSendMailDetail.Where(o => o.ScheduleId == item.Id).ToListAsync();
                    if (users.Any())
                    {


                        var offlineDevices = await _deviceReportService.GetsConnectDeviceReport(item.OrganizationId, "Offline");//offline
                        var onlineDevices = await _deviceReportService.GetsConnectDeviceReport(item.OrganizationId, "Online");//online

                        var retVal = FileExel_ScheduleSendMailDeviceStatusWarning(DateTime.Now, DateTime.Now, item.ScheduleTimeStart.Value, item.ScheduleTimeEnd.Value, item.OrganizationName, offlineDevices, onlineDevices);

                        if (retVal.Succeeded)
                        {
                            bool fileAvaiable = File.Exists(retVal.Data.FileFullName);

                            // Neu tim thay file
                            if (fileAvaiable)
                            {
                                string email_sender_id = "1";
                                EmailConfiguration emailConfig = await _dbContext.EmailConfiguration.Where(o => o.OrganizationId == item.OrganizationId).FirstOrDefaultAsync();

                                if (emailConfig == null)
                                    emailConfig = new EmailConfiguration()
                                    {
                                        Email = _configuration["MailSettings:EmailFrom"],
                                        Server = _configuration["MailSettings:SmtpHost"],
                                        Port = int.Parse(_configuration["MailSettings:SmtpPort"]),
                                        PassWord = _configuration["MailSettings:SmtpPass"],
                                        UserName = _configuration["MailSettings:DisplayName"]
                                    };

                                if (emailConfig == null)
                                    break;

                                email_sender_id = emailConfig.Id;
                                foreach (var user in users)
                                {
                                    //// Chuyen file thanh Json Array
                                    //var arrayFile = new[] { fileName, fileName };
                                    //string jsonFile = JsonConvert.SerializeObject(arrayFile);

                                    string bodyContent = item.ScheduleContentEmail;
                                    bodyContent = bodyContent.Replace("NAME", user.ScheduleUserName).Replace("! ", "! </br>").Replace(". ", ". </br>");
                                    //bodyContent = string.Format(bodyContent, user.ScheduleUserName);

                                    var sendEmail = new SendEmails()
                                    {
                                        OrganizationId = item.OrganizationId ?? "6",
                                        Sent = false,
                                        CreatedDate = now,
                                        EmailSenderId = email_sender_id,
                                        Subject = $"{item.ScheduleTitleEmail} _ {DateTime.Now.ToString("yyyy-MM-dd")}", //emailHeader.subject,
                                        ToEmails = user.ScheduleEmail,
                                        Body = bodyContent,
                                        AttachFile = retVal.Data.FileName
                                    };

                                    await _dbContext.SendEmail.AddAsync(sendEmail);
                                    await _dbContext.SaveChangesAsync();

                                    var attachFiles = new List<string>() { retVal.Data.FileFullName };
                                    var retVal1 = await _sendEmailMessageService1.SendByEventBusAsync(
                                                    new SendEmailMessageRequest1()
                                                    {
                                                        Id = sendEmail.Id.ToString(),
                                                        Message = new MailRequest()
                                                        {
                                                            ToEmail = user.ScheduleEmail,
                                                            EmailSubject = $"{item.ScheduleTitleEmail}_{DateTime.Now.ToString("yyyy-MM-dd")}",
                                                            EmailBody = bodyContent,
                                                            AttachFiles = attachFiles,
                                                        },

                                                        EmailSettings = new EmailSettings()
                                                        {
                                                            EmailFrom = emailConfig.Email,
                                                            SmtpHost = emailConfig.Server,
                                                            SmtpPort = emailConfig.Port ?? 587,
                                                            SmtpPass = emailConfig.PassWord,
                                                            SmtpUser = emailConfig.Email,
                                                            DisplayName = emailConfig.UserName,
                                                        }
                                                    });
                                    if (retVal1.Succeeded)
                                        item.ReportSent = true;

                                }
                            }
                        }
                        else
                        {
                            Logger.Error(retVal.Message);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        finally
        {
            Is_CheckDeviceStatusWarning = false;
        }

    }

    private Result<Footfall_ReportFile> FileExel_ScheduleSendMailDeviceStatusWarning(DateTime start_date, DateTime end_date, TimeSpan start_time, TimeSpan end_time, string? org_name, List<DeviceStatusWarningModel> offlineDevices, List<DeviceStatusWarningModel> onlineDevices)
    {
        try
        {
            var rootParth = Common.GetExcelFolder();
            var rootParth_Output = Common.GetExcelDateFullFolder(DateTime.Now.Date);
            var parth_Output = Common.GetExcelDatePathFolder(DateTime.Now.Date);

            string str_startdate = start_date.ToString("yyyy-MM-dd");
            string str_enddate = end_date.ToString("yyyy-MM-dd");

            int start_hour = start_time.Hours;
            int end_hour = end_time.Hours;


            string inputFile = rootParth + "BAOCAOTRANGTHAITHIETBI.xlsx";
            string time_report = $"{start_time.ToString(@"hh\:mm")}" + " - " + end_time.ToString(@"hh\:mm");



            if (File.Exists(inputFile))
            {
                string nameFile = $"BAOCAOTRANGTHAITHIETBI{DateTime.Now.Ticks}.xlsx";
                string outputFile = rootParth_Output + nameFile;
                string fileName = parth_Output + nameFile;

                using (var template = new XLTemplate(inputFile))
                {
                    var data1 = new
                    {
                        date_report = $"{str_startdate}",
                        time_report = $"{time_report}",
                        org_name
                    };
                    template.AddVariable("items", offlineDevices);
                    template.AddVariable("datas", onlineDevices);
                    template.AddVariable(data1);
                    template.Generate();
                    template.SaveAs(outputFile);
                }
                var file = new Footfall_ReportFile()
                {
                    FileName = fileName,
                    FileFullName = outputFile
                };

                return new Result<Footfall_ReportFile>(file, "Thành công", true);
            }
            return new Result<Footfall_ReportFile>($"Không tìm thấy File template: {inputFile}", false);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return new Result<Footfall_ReportFile>(e.Message, false);
        }
    }
    #endregion

}
