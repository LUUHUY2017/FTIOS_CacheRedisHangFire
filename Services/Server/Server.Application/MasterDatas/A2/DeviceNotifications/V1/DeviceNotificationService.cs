using AMMS.Notification.Commons;
using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Application.CronJobs;
using Server.Application.MasterDatas.A2.DeviceNotifications.V1.Models;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.DashBoardReports.Models;
using Server.Core.Interfaces.A2.DeviceNotifications;
using Server.Core.Interfaces.A2.Organizations;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Application.MasterDatas.A2.DeviceNotifications.V1
{
    public class DeviceNotificationService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ICronJobService _IConJobService;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IDeviceNotificationRepository _notificationRepository;
        private readonly IMasterDataDbContext _dbContext;

        public DeviceNotificationService(
            IMapper mapper,
            IMediator mediator,
            IOrganizationRepository organizationRepository,
            ICronJobService iConJobService,
            IRecurringJobManager recurringJobManager,
            IDeviceNotificationRepository deviceNotificationRepository,
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

        public async Task<Result<List<DeviceNotificationResponse>>> Filter(DeviceNotificationModel filter)
        {
            try
            {
                filter.Note = NotificationConst.CANHBAOTHIETBIMATKETNOI;
                var datas = await _notificationRepository.GetAlls(filter);
                var result = _mapper.Map<List<DeviceNotificationResponse>>(datas);
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
                return new Result<List<DeviceNotificationResponse>>(result, "Thành công!", true);
            }
            catch (Exception ex)
            {
                return new Result<List<DeviceNotificationResponse>>(null, $"Có lỗi: {ex.Message}", false);
            }
        }


        public async Task<Result<ScheduleSendMail>> Edit(DeviceNotificationRequest request)
        {
            try
            {
                var model = _mapper.Map<ScheduleSendMail>(request);
                model.ScheduleNote = NotificationConst.CANHBAOTHIETBIMATKETNOI;
                var retVal = await _notificationRepository.UpdateAsync(model);

                try
                {
                    if (retVal.Succeeded)
                    {
                        var newCronExpression = "0 * * * *";
                        var timeSentHour = retVal.Data.ScheduleTimeStart.Value.Hours;
                        var endSentHour = retVal.Data.ScheduleTimeEnd.Value.Hours;

                        if (retVal.Data.ScheduleNote == NotificationConst.CANHBAOTHIETBIMATKETNOI)
                        {
                            if (request.ScheduleSequentialSending == "Hourly")
                            {
                                //newCronExpression = $" */5 {timeSentHour}-{endSentHour} * * *";
                                newCronExpression = $" 0 {timeSentHour}-{endSentHour} * * *";
                                _IConJobService.UpdateDeviceStatusWarningCronJob(NotificationConst.CANHBAOTHIETBIMATKETNOI, retVal.Data.Id, newCronExpression);
                            }
                            if (request.ScheduleSequentialSending == "TwoHourly")
                            {
                                newCronExpression = $"0 {timeSentHour}-{endSentHour}/{2} * * *";
                                _IConJobService.UpdateDeviceStatusWarningCronJob(NotificationConst.CANHBAOTHIETBIMATKETNOI, retVal.Data.Id, newCronExpression);
                            }
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
                var result = await _notificationRepository.ActiveAsync(request);
                try
                {
                    if (result.Succeeded)
                    {
                        var newCronExpression = "0 * * * *";
                        var timeSentHour = result.Data.ScheduleTimeStart.Value.Hours;
                        var endSentHour = result.Data.ScheduleTimeEnd.Value.Hours;

                        if (result.Data.ScheduleNote == NotificationConst.CANHBAOTHIETBIMATKETNOI)
                        {
                            if (result.Data.ScheduleSequentialSending == "Hourly")
                            {
                                //newCronExpression = $" */5 {timeSentHour}-{endSentHour} * * *";
                                newCronExpression = $" 0 {timeSentHour}-{endSentHour} * * *";
                                _IConJobService.UpdateDeviceStatusWarningCronJob(NotificationConst.CANHBAOTHIETBIMATKETNOI, result.Data.Id, newCronExpression);
                            }
                            if (result.Data.ScheduleSequentialSending == "TwoHourly")
                            {
                                newCronExpression = $"0 {timeSentHour}-{endSentHour}/{2} * * *";
                                _IConJobService.UpdateDeviceStatusWarningCronJob(NotificationConst.CANHBAOTHIETBIMATKETNOI, result.Data.Id, newCronExpression);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new Result<ScheduleSendMail>(null, $"Có lỗi: {ex.Message}", false);
                }
                return result;
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
                    if (retVal.Data.ScheduleSequentialSending == "Hourly")
                    {
                        JobId = NotificationConst.CANHBAOTHIETBIMATKETNOI + "_" + retVal.Data.Id;
                    }
                    else if (retVal.Data.ScheduleSequentialSending == "TwoHourly")
                    {
                        JobId = NotificationConst.CANHBAOTHIETBIMATKETNOI + "_" + retVal.Data.Id;
                    }
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
}
