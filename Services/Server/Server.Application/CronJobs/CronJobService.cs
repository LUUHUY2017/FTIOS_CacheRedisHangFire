using AMMS.Notification.Workers.Emails;
using EventBus.Messages;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.SignalRs;

namespace Server.Application.CronJobs;

public partial class CronJobService : ICronJobService
{
    private readonly ISignalRClientService _signalRService;
    private readonly MasterDataDbContext _biDbContext;
    //private readonly IEventBusAdapter _eventBusAdapter;
    //private readonly MediatR.IMediator _mediator;
    //private readonly SendEmailMessageService1 _sendEmailMessageService1;
    //private readonly IRecurringJobManager _iRecurringJobManager;

    private readonly IConfiguration _configuration;

    public CronJobService(
          ISignalRClientService signalRService
        , MasterDataDbContext biDbContext
        //, MediatR.IMediator mediator
        //, IEventBusAdapter eventBusAdapter
        // , SendEmailMessageService1 sendEmailMessageService1
        //, IRecurringJobManager iRecurringJobManager
        //, IConfiguration configuration
        )
    {

        _signalRService = signalRService;
        _biDbContext = biDbContext;
        //_mediator = mediator;
        //_eventBusAdapter = eventBusAdapter;
        //_sendEmailMessageService1 = sendEmailMessageService1;
        //_iRecurringJobManager = iRecurringJobManager;
        //_configuration = configuration;
    }
    //#region Kiểm tra thiết bị online/offline
    ///// <summary>
    ///// Kiểm tra thiết bị online/offline
    ///// </summary>

    //static bool Is_CheckDeviceOnline { get; set; } = false;
    //public async Task CheckDeviceOnline()
    //{
    //    if (Is_CheckDeviceOnline)
    //        return;

    //    Is_CheckDeviceOnline = true;
    //    try
    //    {
    //        var terminals_cache = await _deviceCacheService.Gets();

    //        var terminals = await _biDbContext.Terminals.Where(o => o.Actived == 1).ToListAsync();

    //        if (terminals_cache != null && terminals_cache.Count() > 0)
    //        {
    //            foreach (var d in terminals_cache)
    //            {
    //                d.LastTimeCheckConnection = DateTime.Now;

    //                if (d.LastTimeUpdateSocket == null || (d.LastTimeCheckConnection - d.LastTimeUpdateSocket).Value.TotalMinutes > 5)
    //                {
    //                    if (d.OnlineStatus == "Online" || string.IsNullOrEmpty(d.OnlineStatus))
    //                    {
    //                        d.OnlineStatus = "Offline";
    //                        d.LastTimeOffline = DateTime.Now;

    //                        await _biDbContext.TerminalStatusLogs.AddAsync(new TerminalStatusLog()
    //                        {
    //                            TerminalId = d.Id,
    //                            CreatedAt = d.LastTimeOffline,
    //                            Status = false,
    //                        });
    //                        //Đẩy dữ liệu vào Queue
    //                        //_eventBusAdapter.GetSendEndpointAsync(EventBus.Messages.Common.EventBusConstants.DeviceOnlineOffline_Queue).Result.Send(d).Wait();
    //                        _eventBusAdapter.GetSendEndpointAsync(_configuration.GetValue<string>("DataArea") + EventBus.Messages.Common.EventBusConstants.DeviceOnlineOffline_Queue).Result.Send(d).Wait();
    //                    }
    //                }


    //                var terminal = terminals.FirstOrDefault(o => o.SerialNumber == d.SerialNumber);
    //                if (terminal != null)
    //                {
    //                    terminal.LastTimeCheckConnection = d.LastTimeCheckConnection;
    //                    terminal.LastTimeUpdateSocket = d.LastTimeUpdateSocket;
    //                    terminal.LastTimeOffline = d.LastTimeOffline;
    //                    terminal.OnlineStatus = d.OnlineStatus;
    //                    terminal.WanIpAddress = d.WanIpAddress;
    //                    terminal.IpAddress = d.IpAddress;
    //                    terminal.HttpPort = d.HttpPort;
    //                    terminal.HttpsPort = d.HttpsPort;
    //                    _biDbContext.Terminals.Update(terminal);

    //                }
    //            }
    //            await _biDbContext.SaveChangesAsync();
    //        }

    //        foreach (var terminal in terminals)
    //            await _deviceCacheService.Add(terminal);

    //        if (terminals_cache != null && terminals_cache.Count() > 0)
    //        {
    //            if (_signalRService != null && _signalRService.Connection != null && _signalRService.Connection.State == Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected)
    //            {
    //                await _signalRService.Connection.InvokeAsync("RefreshDevice", JsonConvert.SerializeObject(terminals));
    //            }
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Logger.Error(e);
    //    }
    //    finally
    //    {
    //        Is_CheckDeviceOnline = false;
    //    }
    //}
    //#endregion

    ///// <summary>
    ///// Update Cronjob
    ///// </summary>
    ///// <param name="jobId"></param>
    ///// <param name="newCronExpression"></param>
    ///// <exception cref="ArgumentException"></exception>        

    //public void CreateScheduleSendMailCronJob(List<ScheduleSendMail> scheduleLists)
    //{
    //    foreach (var item in scheduleLists)
    //    {
    //        var timeSentHour = item.ScheduleTimeSend.HasValue ? item.ScheduleTimeSend.Value.Hours : 0;
    //        var timeSentMinute = item.ScheduleTimeSend.HasValue ? item.ScheduleTimeSend.Value.Hours : 0;
    //        var newCronExpression = "0 * * * *";

    //        if (item.ScheduleSequentialSending == "Daily" && item.ScheduleNote == "BAOCAOTUDONG")
    //        {
    //            newCronExpression = $"{timeSentMinute} {timeSentHour} * * *";
    //            UpdateScheduleSendMailCronJob("ScheduleSendMailReportDaily", item.Id, newCronExpression);
    //        }

    //        if (item.ScheduleSequentialSending == "Monthly" && item.ScheduleNote == "BAOCAOTUDONG")
    //        {
    //            newCronExpression = $"{timeSentMinute} {timeSentHour} 1 * *";
    //            UpdateScheduleSendMailCronJob("ScheduleSendMailReportMonthly", item.Id, newCronExpression);
    //        }

    //        if (item.ScheduleNote == NotificationConst.CANHBAOTHIETBIMATKETNOI)
    //        {
    //            if (item.ScheduleSequentialSending == "Hourly")
    //            {
    //                timeSentHour = item.ScheduleTimeStart.Value.Hours;
    //                var endSentHour = item.ScheduleTimeEnd.Value.Hours;
    //                //newCronExpression = $" */5 {timeSentHour}-{endSentHour} * * *";
    //                newCronExpression = $" 0 {timeSentHour}-{endSentHour} * * *";
    //                UpdateScheduleSendMailCronJob(NotificationConst.CANHBAOTHIETBIMATKETNOI, item.Id, newCronExpression);
    //            }
    //            if (item.ScheduleSequentialSending == "TwoHourly")
    //            {
    //                timeSentHour = item.ScheduleTimeStart.Value.Hours;
    //                var endSentHour = item.ScheduleTimeEnd.Value.Hours;
    //                newCronExpression = $"0 {timeSentHour}-{endSentHour}/{2} * * *";
    //                UpdateScheduleSendMailCronJob(NotificationConst.CANHBAOTHIETBIMATKETNOI, item.Id, newCronExpression);
    //            }
    //        }

    //    }
    //}

    //public void UpdateScheduleSendMailCronJob(string jobId, int sheduleId, string newCronExpression)
    //{
    //    var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
    //    string JobName = jobId + "_" + sheduleId;

    //    if (jobId == "ScheduleSendMailReportDaily")
    //    {
    //        RecurringJob.AddOrUpdate(JobName, () => POC_Report_ScheduleSendMailReportDaily(sheduleId), newCronExpression, TimeZoneInfo.Local);
    //    }
    //    if (jobId == "ScheduleSendMailReportMonthly")
    //    {
    //        RecurringJob.AddOrUpdate(JobName, () => POC_Report_ScheduleSendMailReportMonthly(sheduleId), newCronExpression, TimeZoneInfo.Local);
    //    }
    //    if (jobId == NotificationConst.CANHBAOTHIETBIMATKETNOI)
    //    {
    //        RecurringJob.AddOrUpdate(JobName, () => Device_Warning_ScheduleSendMailHourly(sheduleId), newCronExpression, TimeZoneInfo.Local);
    //    }

    //}

    //#region Báo cáo hàng ngày
    //static bool Is_Run_POC_Report_ScheduleSendMailReportDaily = false;
    //public void POC_Report_ScheduleSendMailReportDaily(int sheduleId)
    //{
    //    //if (!Is_Run_POC_Report_ScheduleSendMailReportDaily)
    //    //{
    //    //    Is_Run_POC_Report_ScheduleSendMailReportDaily = true;
    //    ScheduleSendMailReportDaily(sheduleId).Wait();
    //    //}
    //}
    //public async Task ScheduleSendMailReportDaily(int sheduleId)
    //{
    //    DateTime now = DateTime.Now;
    //    try
    //    {
    //        var datas = await _biDbContext.ScheduleSendMail.Where(o => o.Actived == true && o.ScheduleSequentialSending == "Daily").ToListAsync();
    //        if (datas.Any())
    //        {
    //            foreach (var item in datas)
    //            {
    //                DateTime date = DateTime.Now;
    //                // Nếu dữ liệu lấy ngày hôm trước -1days
    //                if (item.ScheduleDataCollect != "Current")
    //                    date = DateTime.Now.AddDays(-1);

    //                var timeNow = now.Hour;
    //                var timeSend = item.ScheduleTimeSend.Value.Hours;
    //                var start_date = new DateTime(date.Year, date.Month, date.Day, 00, 00, 00);
    //                var end_date = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

    //                //// Reset lại thời gian gửi
    //                //if (now.Hour == 0)
    //                //    item.ReportSent = false;
    //                //// Kiểm tra đúng thời gian cấu hình sẽ gửi
    //                //if (timeNow != timeSend)
    //                //    continue;
    //                //// Nếu thời gian chạy ngắn màn bản tin check đã gửi rồi thì không gửi nữa
    //                //if (timeNow == timeSend && item.ReportSent == true)
    //                //    continue;

    //                if (sheduleId != item.Id)
    //                    continue;

    //                var users = await _biDbContext.ScheduleSendMailDetail.Where(o => o.ScheduleId == item.Id).ToListAsync();
    //                if (users.Any())
    //                {
    //                    var avgtimes = await _biDbContext.PocDataInOutAvgTimes.Where(o =>
    //                                        o.OrganizationId == item.OrganizationId
    //                                        && o.StartTime.Date >= start_date.Date
    //                                        && o.StartTime.Date <= end_date.Date
    //                                        && o.StartTime.Hour >= item.ScheduleTimeStart.Value.Hours
    //                                        && o.StartTime.Hour <= item.ScheduleTimeEnd.Value.Hours)
    //                        .ToListAsync();
    //                    var sites = await _biDbContext.Sites.Where(o => o.Actived == true && o.Deleted != 1 && o.OrganizationId == item.OrganizationId && o.Store == true).ToListAsync();
    //                    var retVal = FileExel_ScheduleSendMailReportDaily(start_date, end_date, item.ScheduleTimeStart.Value, item.ScheduleTimeEnd.Value, item.OrganizationName, sites, avgtimes);

    //                    if (retVal.Succeeded)
    //                    {
    //                        bool fileAvaiable = File.Exists(retVal.Data.FileFullName);

    //                        // Neu tim thay file
    //                        if (fileAvaiable)
    //                        {
    //                            int email_sender_id = 1;
    //                            EmailConfiguration emailConfig = await _biDbContext.EmailConfigurations.Where(o => o.OrganizationId == item.OrganizationId).FirstOrDefaultAsync();

    //                            // Nếu không có sẽ tìm email mặc định
    //                            if (emailConfig == null)
    //                                emailConfig = await _biDbContext.EmailConfigurations.Where(o => o.OrganizationId == 0).FirstOrDefaultAsync();

    //                            if (emailConfig == null)
    //                                break;

    //                            email_sender_id = emailConfig.Id;
    //                            foreach (var user in users)
    //                            {
    //                                //// Chuyen file thanh Json Array
    //                                //var arrayFile = new[] { fileName, fileName };
    //                                //string jsonFile = JsonConvert.SerializeObject(arrayFile);

    //                                string bodyContent = item.ScheduleContentEmail;
    //                                bodyContent = bodyContent.Replace("NAME", user.ScheduleUserName).Replace("! ", "! </br>").Replace(". ", ". </br>");
    //                                //bodyContent = string.Format(bodyContent, user.ScheduleUserName);

    //                                var sendEmail = new Core.Entities.SendEmail()
    //                                {
    //                                    Id = Guid.NewGuid(),
    //                                    OrganizationId = item.OrganizationId ?? 6,
    //                                    Sent = false,
    //                                    CreateTime = now,
    //                                    EmailSenderId = email_sender_id,
    //                                    Subject = $"{item.ScheduleTitleEmail} _ {start_date.ToString("yyyy-MM-dd")}", //emailHeader.subject,
    //                                    ToEmails = user.ScheduleEmail,
    //                                    Body = bodyContent,
    //                                    AttachFile = retVal.Data.FileName
    //                                };

    //                                await _biDbContext.SendEmails.AddAsync(sendEmail);
    //                                await _biDbContext.SaveChangesAsync();

    //                                var attachFiles = new List<string>() { retVal.Data.FileFullName };
    //                                var retVal1 = await _sendEmailMessageService1.SendByEventBusAsync(
    //                                                new SendEmailMessageRequest1()
    //                                                {
    //                                                    Id = sendEmail.Id.ToString(),
    //                                                    Message = new MailRequest()
    //                                                    {
    //                                                        ToEmail = user.ScheduleEmail,
    //                                                        EmailSubject = $"{item.ScheduleTitleEmail}_{start_date.ToString("yyyy-MM-dd")}",
    //                                                        EmailBody = bodyContent,
    //                                                        AttachFiles = attachFiles,
    //                                                    },

    //                                                    EmailSettings = new EmailSettings()
    //                                                    {
    //                                                        EmailFrom = emailConfig.Email,
    //                                                        Host = emailConfig.Server,
    //                                                        Port = emailConfig.Port ?? 587,
    //                                                        Password = emailConfig.PassWord,
    //                                                        UserName = emailConfig.Email,
    //                                                        DisplayName = emailConfig.UserName,
    //                                                    }
    //                                                });
    //                                if (retVal1.Succeeded)
    //                                    item.ReportSent = true;

    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        Logger.Error(retVal.Message);
    //                    }
    //                }
    //                //_biDbContext.ScheduleSendMail.Update(item);
    //                //await _biDbContext.SaveChangesAsync();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Logger.Error(ex);
    //    }
    //    Is_Run_POC_Report_ScheduleSendMailReportDaily = false;

    //}
    //private Result<Footfall_ReportFile> FileExel_ScheduleSendMailReportDaily(DateTime start_date, DateTime end_date, TimeSpan start_time, TimeSpan end_time, string? org_name, List<Site> sites, List<PocDataInOutAvgTime> avg_times)
    //{
    //    try
    //    {
    //        var rootParth = Common.GetExcelFolder();
    //        var rootParth_Output = Common.GetExcelDateFullFolder(DateTime.Now.Date);
    //        var parth_Output = Common.GetExcelDatePathFolder(DateTime.Now.Date);

    //        string str_startdate = start_date.ToString("yyyy-MM-dd");
    //        string str_enddate = end_date.ToString("yyyy-MM-dd");


    //        int start_hour = start_time.Hours;
    //        int end_hour = end_time.Hours;

    //        List<string> list = new List<string>();
    //        for (int i = start_hour; i <= end_hour; i++)
    //        {
    //            TimeSpan result = TimeSpan.FromHours(i);
    //            string fromTimeString = result.ToString(@"hh\:mm");
    //            list.Add(fromTimeString);
    //        }

    //        List<Dictionary<string, object>> dictList = new List<Dictionary<string, object>>();
    //        for (int i = 0; i < sites.Count; i++)
    //        {
    //            Dictionary<string, object> myObject = new Dictionary<string, object>();

    //            myObject["No"] = i + 1;
    //            myObject["Locations"] = sites[i].SiteName;

    //            var dataSites = avg_times.Where(o => o.SiteId == sites[i].Id).OrderByDescending(o => o.StartTime).ToList();
    //            if (dataSites != null && dataSites.Count() > 0)
    //            {
    //                int e = -1;
    //                for (int j = start_hour; j <= end_hour; j++)
    //                {
    //                    e = e + 1;
    //                    int? val = GetDataByHour_1(dataSites, j);
    //                    myObject[$"H{e}"] = val ?? 0;
    //                }

    //                var tt = dataSites.Sum(o => o.NumToEnter);
    //                myObject["Total"] = tt.HasValue ? tt.Value : 0;
    //            }
    //            dictList.Add(myObject);
    //        }

    //        List<Footfall_BaoCao_Ngay_Gio> people = Common.ConvertDictionaryListToObjectList<Footfall_BaoCao_Ngay_Gio>(dictList);
    //        people = people.OrderByDescending(o => o.Total).ToList();

    //        string inputFile = rootParth + "Footfall_report_daily_hours_template.xlsx";
    //        string time_report = $"{start_time.ToString(@"hh\:mm")}" + " - " + end_time.ToString(@"hh\:mm");

    //        if (File.Exists(inputFile))
    //        {
    //            string nameFile = $"Footfall_report_daily_hours_{DateTime.Now.Ticks}.xlsx";
    //            string outputFile = rootParth_Output + nameFile;
    //            string fileName = parth_Output + nameFile;

    //            using (var template = new XLTemplate(inputFile))
    //            {
    //                var data1 = new
    //                {
    //                    date_report = $"{str_startdate}",
    //                    time_report = $"{time_report}",
    //                    org_name,
    //                    items = people
    //                };
    //                template.AddVariable(data1);
    //                template.Generate();

    //                int tongSoDongHeadMacDinh = 10;
    //                int tongSoCotMacDinh = 4 + 1;
    //                int tongSoCotCoTheXayRa = 30;

    //                // Loại bỏ Border và Text của Header
    //                for (int i = tongSoCotMacDinh; i <= tongSoCotCoTheXayRa; i++)
    //                {
    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(9, i).Value = "";

    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(9, i).Style.Border.TopBorder = XLBorderStyleValues.None;
    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(9, i).Style.Border.BottomBorder = XLBorderStyleValues.None;
    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(9, i).Style.Border.LeftBorder = XLBorderStyleValues.None;
    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(9, i).Style.Border.RightBorder = XLBorderStyleValues.None;
    //                }

    //                // Fill Thời gian đã chọn  cột Header
    //                for (int i = 0; i < list.Count(); i++)
    //                {
    //                    int col = 5;
    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(9, col + i).Value = list[i];
    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(9, col + i).Style.Font.Bold = true;

    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(9, col + i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(9, col + i).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(9, col + i).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(9, col + i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
    //                }


    //                // Loại cột tính tổng cuối cùng không có giờ dữ liệu: mặc định File đã tính tổng

    //                int columOutTotal = list.Count() + tongSoCotMacDinh;
    //                int rowTotal = people.Count() + tongSoDongHeadMacDinh;
    //                for (int i = 0; i <= tongSoCotCoTheXayRa; i++)
    //                {
    //                    int colNext = columOutTotal + i;
    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(rowTotal, colNext).Value = "";

    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(rowTotal, colNext).Style.Border.TopBorder = XLBorderStyleValues.None;
    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(rowTotal, colNext).Style.Border.BottomBorder = XLBorderStyleValues.None;
    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(rowTotal, colNext).Style.Border.LeftBorder = XLBorderStyleValues.None;
    //                    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(rowTotal, colNext).Style.Border.RightBorder = XLBorderStyleValues.None;
    //                }


    //                // Loại cột tính tổng cuối cùng không có giờ dữ liệu: mặc định File đã tính tổng
    //                //int rowOutTotal = list.Count() + 5
    //                //int columOutTotal = people.Count() + 10;
    //                //for (int i = rowOutTotal; i <= rowTotal; i++)
    //                //{
    //                //    int colNext = i;
    //                //    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(rowTotal, colNext).Value = "";

    //                //    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(rowTotal, colNext).Style.Border.TopBorder = XLBorderStyleValues.None;
    //                //    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(rowTotal, colNext).Style.Border.BottomBorder = XLBorderStyleValues.None;
    //                //    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(rowTotal, colNext).Style.Border.LeftBorder = XLBorderStyleValues.None;
    //                //    template.Workbook.Worksheets.Worksheet(template.Workbook.Worksheets.First().Name).Cell(rowTotal, colNext).Style.Border.RightBorder = XLBorderStyleValues.None;
    //                //}

    //                template.SaveAs(outputFile);
    //            }
    //            var file = new Footfall_ReportFile()
    //            {
    //                FileName = fileName,
    //                FileFullName = outputFile
    //            };

    //            return new Result<Footfall_ReportFile>(file, "Thành công", true);
    //        }
    //        return new Result<Footfall_ReportFile>($"Không tìm thấy File template: {inputFile}", false);
    //    }
    //    catch (Exception e)
    //    {
    //        Logger.Error(e);
    //        return new Result<Footfall_ReportFile>(e.Message, false);
    //    }
    //}
    //#endregion

    //#region  Báo cáo hàng tháng
    //static bool Is_Run_POC_Report_ScheduleSendMailReportMonthly = false;
    //public void POC_Report_ScheduleSendMailReportMonthly(int sheduleId)
    //{
    //    //if (!Is_Run_POC_Report_ScheduleSendMailReportMonthly)
    //    //{
    //    //    Is_Run_POC_Report_ScheduleSendMailReportMonthly = true;
    //    ScheduleSendMailReportMonthly(sheduleId).Wait();
    //    //}
    //}

    //public async Task ScheduleSendMailReportMonthly(int sheduleId)
    //{
    //    DateTime now = DateTime.Now;
    //    try
    //    {
    //        var datas = await _biDbContext.ScheduleSendMail.Where(o => o.Actived == true && o.ScheduleSequentialSending == "Monthly").ToListAsync();
    //        if (datas.Any())
    //        {
    //            foreach (var item in datas)
    //            {
    //                DateTime date = DateTime.Now;
    //                if (item.ScheduleDataCollect != "Current")
    //                    date = DateTime.Now.AddMonths(-1);

    //                var start_date = new DateTime(date.Year, date.Month, date.Day, 00, 00, 00);
    //                DateTime firtDateOfMonth = new DateTime(start_date.Year, start_date.Month, 1);
    //                DateTime endDateOfMonth = firtDateOfMonth.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

    //                //var timeNow = now.Hour;
    //                //var timeSend = item.ScheduleTimeSend.Value.Hours;

    //                //// Nếu dữ liệu lấy báo cáo của tháng hiện tại
    //                //if (item.ScheduleDataCollect == "Current" && now.Date != endDateOfMonth.Date)
    //                //{
    //                //    continue;
    //                //}
    //                //else if (item.ScheduleDataCollect != "Current" && now.Day != 1)
    //                //{
    //                //    continue;
    //                //}

    //                //// Check giờ gửi === với giờ cấu hình thì chạy
    //                //if (timeNow != timeSend)
    //                //    continue;

    //                if (sheduleId != item.Id)
    //                    continue;

    //                var users = await _biDbContext.ScheduleSendMailDetail.Where(o => o.ScheduleId == item.Id).ToListAsync();
    //                if (users.Any())
    //                {
    //                    var sites = await _biDbContext.Sites.Where(o => o.Actived == true && o.Deleted != 1 && o.OrganizationId == item.OrganizationId && o.Store == true).ToListAsync();
    //                    var avgtimes = _biDbContext.PocDataInOutAvgTimes.Where(o =>
    //                                     o.OrganizationId == item.OrganizationId
    //                                    && o.StartTime.Hour >= item.ScheduleTimeStart.Value.Hours
    //                                    && o.StartTime.Hour <= item.ScheduleTimeEnd.Value.Hours
    //                                    && o.StartTime >= firtDateOfMonth && o.StartTime <= endDateOfMonth
    //                                    ).ToList();

    //                    var retVal = FileExel_ScheduleSendMailReportMonthly(start_date, item.OrganizationName, sites, avgtimes);
    //                    if (retVal.Succeeded)
    //                    {
    //                        string fileName = retVal.Data.FileFullName;
    //                        bool fileAvaiable = File.Exists(fileName);
    //                        // Neu tim thay file
    //                        if (fileAvaiable)
    //                        {
    //                            int email_sender_id = 1;
    //                            EmailConfiguration emailConfig = await _biDbContext.EmailConfigurations.Where(o => o.OrganizationId == item.OrganizationId).FirstOrDefaultAsync();

    //                            // Nếu không có sẽ tìm email mặc định
    //                            if (emailConfig == null)
    //                                emailConfig = await _biDbContext.EmailConfigurations.Where(o => o.OrganizationId == 0).FirstOrDefaultAsync();

    //                            if (emailConfig == null)
    //                                break;

    //                            email_sender_id = emailConfig.Id;
    //                            foreach (var user in users)
    //                            {
    //                                string bodyContent = item.ScheduleContentEmail;
    //                                bodyContent = bodyContent.Replace("NAME", user.ScheduleUserName).Replace("! ", "! </br>").Replace(". ", ". </br>");
    //                                //bodyContent = string.Format(bodyContent, user.ScheduleUserName);

    //                                var sendEmail = new Core.Entities.SendEmail()
    //                                {
    //                                    Id = Guid.NewGuid(),
    //                                    OrganizationId = item.OrganizationId ?? 6,
    //                                    Sent = false,
    //                                    CreateTime = now,
    //                                    EmailSenderId = email_sender_id,
    //                                    Subject = $"{item.ScheduleTitleEmail} _ {start_date.ToString("yyyy-MM-dd")}", //emailHeader.subject,
    //                                    ToEmails = user.ScheduleEmail,
    //                                    Body = bodyContent,
    //                                    AttachFile = retVal.Data.FileName
    //                                };

    //                                await _biDbContext.SendEmails.AddAsync(sendEmail);
    //                                await _biDbContext.SaveChangesAsync();

    //                                var attachFiles = new List<string>() { retVal.Data.FileFullName };
    //                                var retVal1 = await _sendEmailMessageService1.SendByEventBusAsync(
    //                                                new SendEmailMessageRequest1()
    //                                                {
    //                                                    Id = sendEmail.Id.ToString(),
    //                                                    Message = new MailRequest()
    //                                                    {
    //                                                        ToEmail = user.ScheduleEmail,
    //                                                        EmailSubject = $"{item.ScheduleTitleEmail}_{start_date.ToString("yyyy-MM-dd")}",
    //                                                        EmailBody = bodyContent,
    //                                                        AttachFiles = attachFiles,
    //                                                    },
    //                                                    EmailSettings = new EmailSettings()
    //                                                    {
    //                                                        EmailFrom = emailConfig.Email,
    //                                                        Host = emailConfig.Server,
    //                                                        Port = emailConfig.Port ?? 587,
    //                                                        Password = emailConfig.PassWord,
    //                                                        UserName = emailConfig.UserName,
    //                                                        DisplayName = emailConfig.UserName,
    //                                                    }
    //                                                });
    //                                //if (retVal1.Succeeded)
    //                                //{
    //                                //    item.ReportSent = true;
    //                                //    _biDbContext.ScheduleSendMail.Update(item);
    //                                //    await _biDbContext.SaveChangesAsync();
    //                                //}
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Logger.Error(ex);
    //    }
    //    Is_Run_POC_Report_ScheduleSendMailReportMonthly = false;

    //}
    //private Result<Footfall_ReportFile> FileExel_ScheduleSendMailReportMonthly(DateTime start_date, string? org_name, List<Site> sites, List<PocDataInOutAvgTime> avg_times)
    //{
    //    try
    //    {
    //        var rootParth = Common.GetExcelFolder();
    //        var rootParth_Output = Common.GetExcelDateFullFolder(DateTime.Now.Date);
    //        var parth_Output = Common.GetExcelDatePathFolder(DateTime.Now.Date);

    //        List<DateTime> range_months = new List<DateTime>();
    //        DateTime firtDateOfMonth = new DateTime(start_date.Year, start_date.Month, 1);
    //        var totalDayOfMonth = DateTime.DaysInMonth(start_date.Year, start_date.Month);

    //        for (int i = 1; i <= totalDayOfMonth; i++)
    //            range_months.Add(new DateTime(start_date.Year, start_date.Month, i));

    //        DateTime endDateOfMonth = range_months[totalDayOfMonth - 1];
    //        DateTime endDateOfMonth1 = endDateOfMonth.AddHours(23).AddMinutes(59);


    //        string str_startdate = firtDateOfMonth.ToString("yyyy-MM-dd");
    //        string str_enddate = endDateOfMonth.ToString("yyyy-MM-dd");
    //        List<Footfall_BaoCao_Ngay_Gio> dulieu_baocao = new List<Footfall_BaoCao_Ngay_Gio>();
    //        for (int i = 0; i < sites.Count; i++)
    //        {
    //            Footfall_BaoCao_Ngay_Gio d = new Footfall_BaoCao_Ngay_Gio();
    //            d.No = i + 1;
    //            d.Locations = sites[i].SiteName;
    //            var dataSites = avg_times.Where(o => o.SiteId == sites[i].Id).ToList();
    //            if (dataSites != null && dataSites.Count() > 0)
    //            {
    //                d.H1 = GetDataByDay(dataSites, 1);
    //                d.H2 = GetDataByDay(dataSites, 2);
    //                d.H3 = GetDataByDay(dataSites, 3);
    //                d.H4 = GetDataByDay(dataSites, 4);
    //                d.H5 = GetDataByDay(dataSites, 5);
    //                d.H6 = GetDataByDay(dataSites, 6);
    //                d.H7 = GetDataByDay(dataSites, 7);
    //                d.H8 = GetDataByDay(dataSites, 8);
    //                d.H9 = GetDataByDay(dataSites, 9);
    //                d.H10 = GetDataByDay(dataSites, 10);
    //                d.H11 = GetDataByDay(dataSites, 11);
    //                d.H12 = GetDataByDay(dataSites, 12);
    //                d.H13 = GetDataByDay(dataSites, 13);
    //                d.H14 = GetDataByDay(dataSites, 14);
    //                d.H15 = GetDataByDay(dataSites, 15);
    //                d.H16 = GetDataByDay(dataSites, 16);
    //                d.H17 = GetDataByDay(dataSites, 17);
    //                d.H18 = GetDataByDay(dataSites, 18);
    //                d.H19 = GetDataByDay(dataSites, 19);
    //                d.H20 = GetDataByDay(dataSites, 20);
    //                d.H21 = GetDataByDay(dataSites, 21);
    //                d.H22 = GetDataByDay(dataSites, 22);
    //                d.H23 = GetDataByDay(dataSites, 23);
    //                d.H24 = GetDataByDay(dataSites, 24);
    //                d.H25 = GetDataByDay(dataSites, 25);
    //                d.H26 = GetDataByDay(dataSites, 26);
    //                d.H27 = GetDataByDay(dataSites, 27);
    //                d.H28 = GetDataByDay(dataSites, 28);

    //                if (totalDayOfMonth > 28)
    //                    d.H29 = GetDataByDay(dataSites, 29);
    //                else
    //                    d.H29 = null;

    //                if (totalDayOfMonth > 29)
    //                    d.H30 = GetDataByDay(dataSites, 30);
    //                else
    //                    d.H30 = null;

    //                if (totalDayOfMonth > 30)
    //                    d.H31 = GetDataByDay(dataSites, 31);
    //                else
    //                    d.H31 = null;

    //                var tt = dataSites.Sum(o => o.NumToEnter);
    //                d.Total = tt.HasValue ? tt.Value : 0; ;
    //            }

    //            dulieu_baocao.Add(d);
    //        }
    //        dulieu_baocao = dulieu_baocao.OrderByDescending(o => o.Total).ToList();

    //        string inputFile = rootParth + "Footfall_report_monthly_day_template.xlsx";


    //        if (File.Exists(inputFile))
    //        {
    //            string nameFile = $"Footfall_report_monthly_{DateTime.Now.Ticks}.xlsx";
    //            string outputFile = rootParth_Output + nameFile;
    //            string fileName = parth_Output + nameFile;

    //            using (var template = new XLTemplate(inputFile))
    //            {
    //                int k = 0;
    //                var data1 = new
    //                {
    //                    date_report = $"{str_startdate} - {str_enddate}",
    //                    time_report = "08:00 - 22:59",
    //                    org_name = org_name,

    //                    d1 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d2 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d3 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d4 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d5 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d6 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d7 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d8 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d9 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d10 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d11 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d12 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d13 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d14 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d15 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d16 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d17 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d18 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d19 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d20 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d21 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d22 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d23 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d24 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d25 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d26 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d27 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d28 = GetDateString(range_months[k++], totalDayOfMonth),
    //                    d29 = totalDayOfMonth > 28 ? GetDateString(range_months[k++], totalDayOfMonth) : "",
    //                    d30 = totalDayOfMonth > 29 ? GetDateString(range_months[k++], totalDayOfMonth) : "",
    //                    d31 = totalDayOfMonth > 30 ? GetDateString(range_months[k++], totalDayOfMonth) : "",

    //                    items = dulieu_baocao
    //                };
    //                template.AddVariable(data1);
    //                template.Generate();
    //                template.SaveAs(outputFile);
    //            }

    //            var file = new Footfall_ReportFile()
    //            {
    //                FileName = fileName,
    //                FileFullName = outputFile
    //            };

    //            return new Result<Footfall_ReportFile>(file, "Thành công", true);
    //        }
    //        return new Result<Footfall_ReportFile>("Không tìm thấy File template", false);
    //    }
    //    catch (Exception e)
    //    {
    //        Logger.Error(e);
    //        return new Result<Footfall_ReportFile>(e.Message, false);
    //    }
    //}
    //#endregion

    //#region Cảnh báo thiết bị
    //static bool Is_Run_Device_Warning_SendMailHourly = false;
    //public void Device_Warning_ScheduleSendMailHourly(int sheduleId)
    //{
    //    //if (!Is_Run_POC_Report_ScheduleSendMailReportDaily)
    //    //{
    //    //    Is_Run_POC_Report_ScheduleSendMailReportDaily = true;
    //    ScheduleSendMailDeviceWarningHourly(sheduleId).Wait();
    //    //}
    //}
    //public async Task ScheduleSendMailDeviceWarningHourly(int sheduleId)
    //{
    //    DateTime now = DateTime.Now;
    //    try
    //    {
    //        var datas = await _biDbContext.ScheduleSendMail.Where(o => o.Actived == true && o.Id == sheduleId).ToListAsync();
    //        if (datas.Any())
    //        {
    //            foreach (var item in datas)
    //            {
    //                DateTime date = DateTime.Now;
    //                if (sheduleId != item.Id)
    //                    continue;

    //                var users = await _biDbContext.ScheduleSendMailDetail.Where(o => o.ScheduleId == item.Id).ToListAsync();
    //                if (users.Any())
    //                {


    //                    var offlineDevices = await _deviceCacheService.GetsConnectDeviceReport(item.OrganizationId, "Offline");//offline
    //                    var onlineDevices = await _deviceCacheService.GetsConnectDeviceReport(item.OrganizationId, "Online");//online


    //                    var sites = await _biDbContext.Sites.Where(o => o.Actived == true && o.Deleted != 1 && o.OrganizationId == item.OrganizationId && o.Store == true).ToListAsync();
    //                    var retVal = FileExel_ScheduleSendMailDeviceWarningHourly(DateTime.Now, DateTime.Now, item.ScheduleTimeStart.Value, item.ScheduleTimeEnd.Value, item.OrganizationName, sites, offlineDevices, onlineDevices);

    //                    if (retVal.Succeeded)
    //                    {
    //                        bool fileAvaiable = File.Exists(retVal.Data.FileFullName);

    //                        // Neu tim thay file
    //                        if (fileAvaiable)
    //                        {
    //                            int email_sender_id = 1;
    //                            EmailConfiguration emailConfig = await _biDbContext.EmailConfigurations.Where(o => o.OrganizationId == item.OrganizationId).FirstOrDefaultAsync();

    //                            // Nếu không có sẽ tìm email mặc định
    //                            if (emailConfig == null)
    //                                emailConfig = await _biDbContext.EmailConfigurations.Where(o => o.OrganizationId == 0).FirstOrDefaultAsync();

    //                            if (emailConfig == null)
    //                                emailConfig = new EmailConfiguration() {
    //                                    Email = _configuration["MailSettings:EmailFrom"],
    //                                    Server = _configuration["MailSettings:SmtpHost"],
    //                                    Port = int.Parse(_configuration["MailSettings:SmtpPort"]),
    //                                    PassWord = _configuration["MailSettings:SmtpPass"],
    //                                    UserName = _configuration["MailSettings:DisplayName"]
    //                                };

    //                            if (emailConfig == null)
    //                                break;

    //                            email_sender_id = emailConfig.Id;
    //                            foreach (var user in users)
    //                            {
    //                                //// Chuyen file thanh Json Array
    //                                //var arrayFile = new[] { fileName, fileName };
    //                                //string jsonFile = JsonConvert.SerializeObject(arrayFile);

    //                                string bodyContent = item.ScheduleContentEmail;
    //                                bodyContent = bodyContent.Replace("NAME", user.ScheduleUserName).Replace("! ", "! </br>").Replace(". ", ". </br>");
    //                                //bodyContent = string.Format(bodyContent, user.ScheduleUserName);

    //                                var sendEmail = new Core.Entities.SendEmail()
    //                                {
    //                                    Id = Guid.NewGuid(),
    //                                    OrganizationId = item.OrganizationId ?? 6,
    //                                    Sent = false,
    //                                    CreateTime = now,
    //                                    EmailSenderId = email_sender_id,
    //                                    Subject = $"{item.ScheduleTitleEmail} _ {DateTime.Now.ToString("yyyy-MM-dd")}", //emailHeader.subject,
    //                                    ToEmails = user.ScheduleEmail,
    //                                    Body = bodyContent,
    //                                    AttachFile = retVal.Data.FileName
    //                                };

    //                                await _biDbContext.SendEmails.AddAsync(sendEmail);
    //                                await _biDbContext.SaveChangesAsync();

    //                                var attachFiles = new List<string>() { retVal.Data.FileFullName };
    //                                var retVal1 = await _sendEmailMessageService1.SendByEventBusAsync(
    //                                                new SendEmailMessageRequest1()
    //                                                {
    //                                                    Id = sendEmail.Id.ToString(),
    //                                                    Message = new MailRequest()
    //                                                    {
    //                                                        ToEmail = user.ScheduleEmail,
    //                                                        EmailSubject = $"{item.ScheduleTitleEmail}_{DateTime.Now.ToString("yyyy-MM-dd")}",
    //                                                        EmailBody = bodyContent,
    //                                                        AttachFiles = attachFiles,
    //                                                    },

    //                                                    EmailSettings = new EmailSettings()
    //                                                    {
    //                                                        EmailFrom = emailConfig.Email,
    //                                                        Host = emailConfig.Server,
    //                                                        Port = emailConfig.Port ?? 587,
    //                                                        Password = emailConfig.PassWord,
    //                                                        UserName = emailConfig.Email,
    //                                                        DisplayName = emailConfig.UserName,
    //                                                    }
    //                                                });
    //                                if (retVal1.Succeeded)
    //                                    item.ReportSent = true;

    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        Logger.Error(retVal.Message);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Logger.Error(ex);
    //    }
    //    Is_Run_POC_Report_ScheduleSendMailReportDaily = false;

    //}

    //private Result<Footfall_ReportFile> FileExel_ScheduleSendMailDeviceWarningHourly(DateTime start_date, DateTime end_date, TimeSpan start_time, TimeSpan end_time, string? org_name, List<Site> sites, List<DisconnectDeviceReport> offlineDevices, List<DisconnectDeviceReport> onlineDevices)
    //{
    //    try
    //    {
    //        var rootParth = Common.GetExcelFolder();
    //        var rootParth_Output = Common.GetExcelDateFullFolder(DateTime.Now.Date);
    //        var parth_Output = Common.GetExcelDatePathFolder(DateTime.Now.Date);

    //        string str_startdate = start_date.ToString("yyyy-MM-dd");
    //        string str_enddate = end_date.ToString("yyyy-MM-dd");

    //        int start_hour = start_time.Hours;
    //        int end_hour = end_time.Hours;


    //        string inputFile = rootParth + "Footfall_deviceDisConnect_daily_hours_template.xlsx";
    //        string time_report = $"{start_time.ToString(@"hh\:mm")}" + " - " + end_time.ToString(@"hh\:mm");



    //        if (File.Exists(inputFile))
    //        {
    //            string nameFile = $"Footfall_deviceDisConnect_daily_{DateTime.Now.Ticks}.xlsx";
    //            string outputFile = rootParth_Output + nameFile;
    //            string fileName = parth_Output + nameFile;

    //            using (var template = new XLTemplate(inputFile))
    //            {
    //                var data1 = new
    //                {
    //                    date_report = $"{str_startdate}",
    //                    time_report = $"{time_report}",
    //                    org_name
    //                };
    //                template.AddVariable("items", offlineDevices);
    //                template.AddVariable("datas", onlineDevices);
    //                template.AddVariable(data1);
    //                template.Generate();
    //                template.SaveAs(outputFile);
    //            }
    //            var file = new Footfall_ReportFile()
    //            {
    //                FileName = fileName,
    //                FileFullName = outputFile
    //            };

    //            return new Result<Footfall_ReportFile>(file, "Thành công", true);
    //        }
    //        return new Result<Footfall_ReportFile>($"Không tìm thấy File template: {inputFile}", false);
    //    }
    //    catch (Exception e)
    //    {
    //        Logger.Error(e);
    //        return new Result<Footfall_ReportFile>(e.Message, false);
    //    }
    //}
    //#endregion


    //#region Cache
    //static bool Is_Run_POC_Report_ScheduleReSendMailReport = false;
    //public void POC_Report_ScheduleReSendMailReport()
    //{
    //    if (!Is_Run_POC_Report_ScheduleReSendMailReport)
    //    {
    //        Is_Run_POC_Report_ScheduleReSendMailReport = true;
    //        ScheduleReSendMailReport().Wait();
    //    }
    //}
    //public async Task ScheduleReSendMailReport()
    //{
    //    DateTime now = DateTime.Now;
    //    try
    //    {
    //        var datas = await _biDbContext.SendEmails.Where(o => o.Sent == null).ToListAsync();
    //        if (datas.Any())
    //        {
    //            foreach (var item in datas)
    //            {
    //                string fileName = item.AttachFile;
    //                string fullName = Common.GetCurentFolder() + fileName;
    //                bool fileAvaiable = File.Exists(fullName);
    //                if (fileAvaiable)
    //                {
    //                    int email_sender_id = 1;
    //                    EmailConfiguration emailConfig = await _biDbContext.EmailConfigurations.Where(o => o.OrganizationId == item.OrganizationId).FirstOrDefaultAsync();

    //                    // Nếu không có sẽ tìm email mặc định
    //                    if (emailConfig == null)
    //                        emailConfig = await _biDbContext.EmailConfigurations.Where(o => o.OrganizationId == 0).FirstOrDefaultAsync();

    //                    if (emailConfig == null)
    //                        break;

    //                    email_sender_id = emailConfig.Id;
    //                    var attachFiles = new List<string>() { fileName };
    //                    var retVal1 = await _sendEmailMessageService1.SendByEventBusAsync(
    //                                    new SendEmailMessageRequest1()
    //                                    {
    //                                        Id = item.Id.ToString(),
    //                                        Message = new MailRequest()
    //                                        {
    //                                            ToEmail = item.ToEmails,
    //                                            EmailSubject = $"{item.Subject}",
    //                                            EmailBody = $"{item.Body}",
    //                                            AttachFiles = attachFiles,
    //                                        },
    //                                        EmailSettings = new EmailSettings()
    //                                        {
    //                                            EmailFrom = emailConfig.Email,
    //                                            Host = emailConfig.Server,
    //                                            Port = emailConfig.Port ?? 587,
    //                                            Password = emailConfig.PassWord,
    //                                            UserName = emailConfig.Email,
    //                                            DisplayName = emailConfig.UserName,
    //                                        }
    //                                    });
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    { Logger.Error(ex); }
    //    Is_Run_POC_Report_ScheduleReSendMailReport = false;
    //}
    //#endregion

    //private int GetDataByHour(List<PocDataInOutAvgTime> datas, int hour)
    //{
    //    var x = datas.FirstOrDefault(o => o.StartTime.Hour == hour);
    //    return x == null ? 0 : x.NumToEnter.HasValue ? x.NumToEnter.Value : 0;
    //}
    //private int? GetDataByHour_1(List<PocDataInOutAvgTime> datas, int hour)
    //{
    //    int? val = null;
    //    var x = datas.FirstOrDefault(o => o.StartTime.Hour == hour);
    //    if (x != null)
    //        val = x.NumToEnter.HasValue ? x.NumToEnter.Value : null;
    //    return val;
    //}

    //private string GetDateString(DateTime date, int totalDayOfMonth)
    //{
    //    if (date.Day <= totalDayOfMonth)
    //        return date.ToString("MM/dd/yyyy");
    //    return "";
    //}
    //private int GetDataByDay(List<PocDataInOutAvgTime> datas, int hour)
    //{
    //    var x = datas.Where(o => o.StartTime.Day == hour).Sum(o => o.NumToEnter);
    //    return x == null ? 0 : (x.HasValue ? x.Value : 0);
    //}


}
