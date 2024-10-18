using AMMS.Notification.Commons;
using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Application.CronJobs;
using Server.Application.MasterDatas.AutoReportInOut.V1.Models;
using Server.Core.Entities;
using Server.Core.Interfaces.Organizations;
using Server.Core.Interfaces.ScheduleSendEmails;
using Server.Core.Interfaces.ScheduleSendEmails.Requests;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;
using Shared.Core.Identity.Object;

namespace Server.Application.MasterDatas.AutoReportInOut.V1;

public class AutoReportInOutService
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IConJobService _IConJobService;
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IScheduleSendMailRepository _sheduleSendEmailReponsity;
    private readonly IMasterDataDbContext _biDbContext;

    public AutoReportInOutService(
        IMapper mapper,
        IMediator mediator,
        IOrganizationRepository organizationRepository,
        IConJobService iConJobService,
        IRecurringJobManager recurringJobManager,
        IScheduleSendMailRepository sheduleSendEmailReponsity,
        IMasterDataDbContext biDbContext
        )
    {
        _mapper = mapper;
        _mediator = mediator;
        _organizationRepository = organizationRepository;
        _IConJobService = iConJobService;
        _recurringJobManager = recurringJobManager;
        _sheduleSendEmailReponsity = sheduleSendEmailReponsity;
        _biDbContext = biDbContext;
    }

    public async Task<Result<List<ScheduleReportInOutResponse>>> Filter(ScheduleSendEmailModel filter)
    {
        try
        {
            bool active = filter.Actived == "1";
            var _data = await (from _do in _biDbContext.ScheduleSendMail
                               where _do.Actived == active
                               && _do.ScheduleNote == NotificationConst.BAOCAOSOSANH
                               && (filter.OrganizationId != 0 ? _do.OrganizationId == filter.OrganizationId : true)
                               && (!string.IsNullOrWhiteSpace(filter.Key) && filter.ColumnTable == "ScheduleName" ? _do.ScheduleName.Contains(filter.Key) : true)
                               select _do).ToListAsync();

            var result = _mapper.Map<List<ScheduleReportInOutResponse>>(_data);
            if (result.Any())
            {
                foreach (var item in result)
                {
                    item.ScheduleExportTypeName = ListCategory.ExportType.FirstOrDefault(o => o.Id == item.ScheduleExportType)?.Name;
                    item.ScheduleReportTypeName = ListCategory.ReportTypeInOut.FirstOrDefault(o => o.Id == item.ScheduleReportType)?.Name;
                    item.ScheduleSequentialSendingName = ListCategory.SequentialSending.FirstOrDefault(o => o.Id == item.ScheduleSequentialSending)?.Name;
                    item.ScheduleDataCollectName = ListCategory.DataCollectType.FirstOrDefault(o => o.Id == item.ScheduleDataCollect)?.Name;
                }
            }
            return new Result<List<ScheduleReportInOutResponse>>(result, "Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<ScheduleReportInOutResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    //public void test()
    //{
    //    var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
    //    foreach (var item in recurringJobs)
    //    {
    //        if (item.Id.StartsWith(jobId, StringComparison.OrdinalIgnoreCase) || jobId.StartsWith(item.Id, StringComparison.OrdinalIgnoreCase))
    //            RecurringJob.RemoveIfExists(item.Id);
    //    }
    //}

    public async Task<Result<ScheduleSendMail>> Edit(ScheduleReportInOutRequest request)
    {
        try
        {
            var model = _mapper.Map<ScheduleSendMail>(request);
            model.ScheduleNote = NotificationConst.BAOCAOSOSANH;
            var retVal = await _sheduleSendEmailReponsity.UpdateAsync(model);

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
                        _IConJobService.UpdateScheduleReportInOutCronJob("GuiEmailCanhBaoHomNaySoVoiHomQua", retVal.Data.Id, newCronExpression);
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
            var retVal = await _sheduleSendEmailReponsity.ActiveAsync(request);
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
                        _IConJobService.UpdateScheduleReportInOutCronJob("GuiEmailCanhBaoHomNaySoVoiHomQua", retVal.Data.Id, newCronExpression);
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
            var retVal = await _sheduleSendEmailReponsity.InActiveAsync(request);
            if (retVal.Succeeded)
            {
                string JobId = "";
                JobId = "GuiEmailCanhBaoHomNaySoVoiHomQua" + "_" + retVal.Data.Id;
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
