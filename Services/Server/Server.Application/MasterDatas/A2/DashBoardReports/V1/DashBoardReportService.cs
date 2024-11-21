using AMMS.Notification.Commons;
using AutoMapper;
using Hangfire;
using MediatR;
using Server.Application.CronJobs;
using Server.Application.MasterDatas.A2.DashBoardReports.V1.Models;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.DashBoardReports;
using Server.Core.Interfaces.A2.DashBoardReports.Models;
using Server.Core.Interfaces.A2.Organizations;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Application.MasterDatas.A2.DashBoardReports.V1;

public class DashBoardReportService
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly ICronJobService _IConJobService;
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IDashBoardReportRepository _notificationRepository;
    private readonly IMasterDataDbContext _dbContext;

    public DashBoardReportService(
        IMapper mapper,
        IMediator mediator,
        IOrganizationRepository organizationRepository,
        ICronJobService iConJobService,
        IRecurringJobManager recurringJobManager,
        IDashBoardReportRepository deviceNotificationRepository,
        IMasterDataDbContext dbContext
        )
    {
        _mapper = mapper;
        _mediator = mediator;
        _organizationRepository = organizationRepository;
        _IConJobService = iConJobService;
        _recurringJobManager = recurringJobManager;
        _notificationRepository = deviceNotificationRepository;
        _dbContext = dbContext;
    }

    public async Task<Result<List<DashBoardReportResponse>>> Filter(DashBoardReportModel filter)
    {
        try
        {
            filter.Note = NotificationConst.BAOCAOTONGQUAN;
            var datas = await _notificationRepository.GetAlls(filter);
            var result = _mapper.Map<List<DashBoardReportResponse>>(datas);
            if (result.Any())
            {
                foreach (var item in result)
                {
                    item.ScheduleExportTypeName = ListScheduleEmailCategory.ExportType.FirstOrDefault(o => o.Id == item.ScheduleExportType)?.Name;
                    item.ScheduleReportTypeName = ListScheduleEmailCategory.ReportType.FirstOrDefault(o => o.Id == item.ScheduleReportType)?.Name;
                    item.ScheduleSequentialSendingName = ListScheduleEmailCategory.SequentialSending.FirstOrDefault(o => o.Id == item.ScheduleSequentialSending)?.Name;
                    item.ScheduleDataCollectName = ListScheduleEmailCategory.DataCollectType.FirstOrDefault(o => o.Id == item.ScheduleDataCollect)?.Name;
                }
            }
            return new Result<List<DashBoardReportResponse>>(result, "Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<DashBoardReportResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }


    public async Task<Result<ScheduleSendMail>> Edit(DashBoardReportRequest request)
    {
        try
        {
            var model = _mapper.Map<ScheduleSendMail>(request);
            model.ScheduleNote = NotificationConst.BAOCAOSOSANH;
            var retVal = await _notificationRepository.UpdateAsync(model);

            try
            {
                if (retVal.Succeeded)
                {
                    var newCronExpression = "0 * * * *";
                    var timeSentHour = retVal.Data.ScheduleTimeSend.HasValue ? retVal.Data.ScheduleTimeSend.Value.Hours : 0;
                    var timeSentMinute = retVal.Data.ScheduleTimeSend.HasValue ? retVal.Data.ScheduleTimeSend.Value.Minutes : 0;

                    if (retVal.Data.ScheduleNote == NotificationConst.BAOCAOSOSANH)
                    {
                        newCronExpression = $"{timeSentMinute} {timeSentHour} * * *";
                        _IConJobService.UpdateDashBoardReportCronJob("GuiBaoCaoTongQuanHeThong", retVal.Data.Id, newCronExpression);
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result<ScheduleSendMail>(null, $"Có lỗi: {ex.Message}", false);
            }

            return retVal;
        }
        catch (Exception ex)
        {
            return new Result<ScheduleSendMail>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<ScheduleSendMail>> Active(ActiveRequest request)
    {
        try
        {
            var retVal = await _notificationRepository.ActiveAsync(request);
            try
            {
                if (retVal.Succeeded)
                {
                    var newCronExpression = "0 * * * *";
                    var timeSentHour = retVal.Data.ScheduleTimeSend.HasValue ? retVal.Data.ScheduleTimeSend.Value.Hours : 0;
                    var timeSentMinute = retVal.Data.ScheduleTimeSend.HasValue ? retVal.Data.ScheduleTimeSend.Value.Minutes : 0;

                    if (retVal.Data.ScheduleNote == NotificationConst.BAOCAOSOSANH)
                    {
                        newCronExpression = $"{timeSentMinute} {timeSentHour} * * *";
                        _IConJobService.UpdateDashBoardReportCronJob("GuiBaoCaoTongQuanHeThong", retVal.Data.Id, newCronExpression);
                    }
                }
            }   
            catch (Exception ex)
            {
                return new Result<ScheduleSendMail>(null, $"Có lỗi: {ex.Message}", false);
            }
            return retVal;
        }
        catch (Exception ex)
        {
            return new Result<ScheduleSendMail>(null, $"Có lỗi: {ex.Message}", false);
        }

    }

    public async Task<Result<ScheduleSendMail>> Inactive(InactiveRequest request)
    {
        try
        {
            var retVal = await _notificationRepository.InActiveAsync(request);
            if (retVal.Succeeded)
            {
                string JobId = "";
                JobId = "GuiBaoCaoTongQuanHeThong" + "_" + retVal.Data.Id;
                _recurringJobManager.RemoveIfExists(JobId);
            }
            return retVal;
        }
        catch (Exception ex)
        {
            return new Result<ScheduleSendMail>(null, $"Có lỗi: {ex.Message}", false);
        }

    }
}