using AMMS.Notification.Commons;
using ClosedXML.Report;
using Hangfire;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using Server.Application.CronJobs.Reponses;
using Server.Application.MasterDatas.A2.DashBoards.V1.Models;
using Server.Core.Entities.A0;
using Server.Core.Entities.A2;
using Shared.Core.Commons;
using Shared.Core.Emails.V1.Commons;
using Shared.Core.Loggers;

namespace Server.Application.CronJobs;

public partial class CronJobService : ICronJobService
{
    #region Báo cáo tổng quan (DashBoard)
    static bool Is_CheckDashBoardReport { get; set; } = false;

    public async Task CreateDashBoardReportCronJob(List<ScheduleSendMail> scheduleLists)
    {
        foreach (var item in scheduleLists)
        {
            var timeSentHour = item.ScheduleTimeSend.HasValue ? item.ScheduleTimeSend.Value.Hours : 0;
            var timeSentMinute = item.ScheduleTimeSend.HasValue ? item.ScheduleTimeSend.Value.Minutes : 0;
            var newCronExpression = "0 * * * *";

            if (item.ScheduleNote == NotificationConst.BAOCAOTONGQUAN)
            {
                newCronExpression = $"{timeSentMinute} {timeSentHour} * * *";
                UpdateDashBoardReportCronJob("GuiBaoCaoTongQuanHeThong", item.Id, newCronExpression);
            }
        }
    }

    public async Task UpdateDashBoardReportCronJob(string jobId, string sheduleId, string newCronExpression)
    {
        var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
        string JobName = jobId + "_" + sheduleId;
        RecurringJob.AddOrUpdate(JobName, () => DashBoard_Report_ScheduleSendMail(sheduleId), newCronExpression, TimeZoneInfo.Local);
    }
    public async Task DashBoard_Report_ScheduleSendMail(string sheduleId)
    {
        ScheduleSendMailDashBoard_Report(sheduleId).Wait();
    }
    public async Task ScheduleSendMailDashBoard_Report(string sheduleId)
    {
        if (Is_CheckDashBoardReport)
            return;

        Is_CheckDashBoardReport = true;

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

                        var filter = new DashBoardFilter()
                        {
                            OrganizationId = item.Id,
                        };
                        var dataReport = await _dashBoardService.DashBoardReport(filter);

                        var retVal = FileExel_ScheduleSendMailDashBoardReport(dataReport.Data);

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
            Is_CheckDashBoardReport = false;
        }

    }

    private Result<Footfall_ReportFile> FileExel_ScheduleSendMailDashBoardReport(TotalDashBoardModel model)
    {
        try
        {
            var rootParth = Common.GetExcelFolder();
            var rootParth_Output = Common.GetExcelDateFullFolder(DateTime.Now.Date);
            var parth_Output = Common.GetExcelDatePathFolder(DateTime.Now.Date);

            string inputFile = rootParth + "BAOCAOTONGQUAN.xlsx";

            if (File.Exists(inputFile))
            {
                string nameFile = $"BAOCAOTONGQUAN{DateTime.Now.Ticks}.xlsx";
                string outputFile = rootParth_Output + nameFile;
                string fileName = parth_Output + nameFile;

                using (var template = new XLTemplate(inputFile))
                {
                    var data = new
                    {
                        date_report = $"{DateTime.Now.ToString("yyyy-MM-dd")}",
                        time_report = $"{DateTime.Now.ToString("HH:mm")}",
                    };
                    template.AddVariable("item", model);
                    template.AddVariable(data);
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

