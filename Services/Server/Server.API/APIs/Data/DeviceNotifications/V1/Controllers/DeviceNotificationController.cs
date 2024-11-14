using AMMS.Notification.Commons;
using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.CronJobs;
using Server.Application.MasterDatas.A2.DeviceNotifications.V1;
using Server.Application.MasterDatas.A2.DeviceNotifications.V1.Models;
using Server.Core.Interfaces.A2.DeviceNotifications;
using Server.Core.Interfaces.A2.ScheduleSendEmails;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.DeviceNotifications.V1.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize("Bearer")]
    [AuthorizeMaster]
    public class DeviceNotificationController : AuthBaseAPIController
    {
        private readonly DeviceNotificationService _deviceNotificationService;
        private readonly IScheduleSendMailRepository _sheduleSendEmailReponsity;
        private readonly ICronJobService _cronJobService;
        private readonly DeviceReportService _deviceReportService;
        public DeviceNotificationController(
            IScheduleSendMailRepository sheduleSendEmailReponsity,
            DeviceNotificationService deviceNotificationService,
            ICronJobService cronJobService,
            DeviceReportService deviceReportService
            )
        {
            _deviceNotificationService = deviceNotificationService;
            _sheduleSendEmailReponsity = sheduleSendEmailReponsity;
            _cronJobService = cronJobService;
            _deviceReportService = deviceReportService;
        }

        /// <summary>
        /// Gửi lại báo cáo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Resend")]
        public async Task<ActionResult> Resend(string id)
        {
            var retVal = await _sheduleSendEmailReponsity.GetById(id);
            if (retVal.Succeeded)
            {
                if (retVal.Data.ScheduleSequentialSending == "Hourly" && retVal.Data.ScheduleNote == NotificationConst.CANHBAOTHIETBIMATKETNOI)
                    await _cronJobService.Device_Warning_ScheduleSendMail(id);
                else if (retVal.Data.ScheduleSequentialSending == "TwoHourly" && retVal.Data.ScheduleNote == NotificationConst.CANHBAOTHIETBIMATKETNOI)
                    await _cronJobService.Device_Warning_ScheduleSendMail(id);
            }
            return Ok();
        }

        /// <summary>
        /// Lấy danh sách
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Gets")]
        public async Task<ActionResult> Filter(DeviceNotificationModel filter)
        {
            var datas = await _deviceNotificationService.Filter(filter);
            return Ok(datas);
        }

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Edit")]
        public async Task<ActionResult> Edit(DeviceNotificationRequest request)
        {
            var datas = await _deviceNotificationService.Edit(request);
            return Ok(datas);
        }

        /// <summary>
        /// Chi tiết lập lịch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Detail")]
        public async Task<ActionResult> Detail(string id)
        {
            return Ok();
        }

        /// <summary>
        /// Kích hoạt
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Active")]
        public async Task<IActionResult> Active([FromBody] ActiveRequest request)
        {
            var datas = await _deviceNotificationService.Active(request);
            return Ok(datas);
        }

        /// <summary>
        /// Ngừng kích hoạt
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Inactive")]
        public async Task<ActionResult> Inactive([FromBody] InactiveRequest request)
        {
            var datas = await _deviceNotificationService.Inactive(request);
            return Ok(datas);
        }

        /// <summary>
        /// Lấy danh sách
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost("GetsConnectDeviceReport")]
        public async Task<ActionResult> GetsConnectDeviceReport(string orgId, string status)
        {
            var datas = await _deviceReportService.GetsConnectDeviceReport(orgId, status);
            return Ok(datas);
        }
    }
}
