using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.API.APIs.Reports.Dashboards.V1.Responses;
using Server.Core.Interfaces.GIO.VehicleInOuts;
using Server.Core.Interfaces.ScheduleSendEmails.Requests;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Reports.Dashboards.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class DashboardController : AuthBaseAPIController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IGIOVehicleInOutRepository _vehicleInOut;

    public DashboardController(
        IMediator mediator,
        IMapper mapper,
        IGIOVehicleInOutRepository vehicleInOut
        )
    {
        _mediator = mediator;
        _mapper = mapper;
        _vehicleInOut = vehicleInOut;

    }


    /// <summary>
    /// Post
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("PostdrawColumnTotal")]
    public async Task<ActionResult> PostdrawColumnTotal(VehicleInOutReportRequest request)
    {
        try
        {
            // ngày hiện tại
            DateTime now = DateTime.Now;
            DateTime fromDate = DateTime.Now.Date;
            DateTime endDate = DateTime.Now.Date.AddDays(1).AddTicks(-1);
            // Theo tuần
            DateTime monday = DateTime.Today.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday).Date;
            DateTime sunday = monday.AddDays(7).Date.AddTicks(-1);

            // Theo tháng
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1).Date;
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddTicks(-1);

            // Theo năm
            DateTime firstDayOfyear = new DateTime(now.Year, 1, 1).Date;
            DateTime lastDayOfYear = new DateTime(now.Year, 12, 31).AddDays(1).AddTicks(-1);

            request.StartDate = firstDayOfyear;
            request.EndDate = lastDayOfYear;
            var data = await _vehicleInOut.GetAlls(request);
            if (data.Succeeded)
            {
                var items = data.Data;

                var todays = items.Where(o => o.DateTimeIn.Value >= fromDate && o.DateTimeIn.Value <= endDate).GroupBy(o => o.LaneInName).Select(o => new ObjectDashboard()
                {
                    Name = o.FirstOrDefault().LaneInName,
                    Value = o.Count()
                }).ToList();


                var weeks = items.Where(o => o.DateTimeIn.Value >= monday && o.DateTimeIn.Value <= sunday).GroupBy(o => o.LaneInName).Select(o => new ObjectDashboard()
                {
                    Name = o.FirstOrDefault().LaneInName,
                    Value = o.Count()
                }).ToList();

                var months = items.Where(o => o.DateTimeIn.Value >= firstDayOfMonth && o.DateTimeIn.Value <= lastDayOfMonth).GroupBy(o => o.LaneInName).Select(o => new ObjectDashboard()
                {
                    Name = o.FirstOrDefault().LaneInName,
                    Value = o.Count()
                }).ToList();

                var years = items.GroupBy(o => o.LaneInName).Select(o => new ObjectDashboard()
                {
                    Name = o.FirstOrDefault().LaneInName,
                    Value = o.Count()
                }).ToList();

                var totals = (from relY in years
                              join _relM in months on relY.Name equals _relM.Name into GW
                              from relM in GW.DefaultIfEmpty()

                              join _relW in weeks on relY.Name equals _relW.Name into GK
                              from relW in GK.DefaultIfEmpty()

                              join _relTo in todays on relY.Name equals _relTo.Name into GY
                              from relTo in GY.DefaultIfEmpty()

                              group new { relTo, relW, relM, relY } by relY.Name into da
                              select new
                              {
                                  Name = da.Key,
                                  Year = da.Sum(o => o.relY.Value),
                                  Today = da.Where(o => o.relTo != null).Sum(o => o.relTo.Value),
                                  Week = da.Where(o => o.relW != null).Sum(o => o.relW.Value),
                                  Month = da.Where(o => o.relM != null).Sum(o => o.relM.Value),
                              }).ToList();

                var result = new
                {
                    todays = todays,
                    weeks = weeks,
                    months = months,
                    years = years,
                    totals = totals,
                };
                return Ok(new Result<object>(result, "Thành công", true));
            }
            else
            {
                return Ok(new Result<object>("Không có dữ liệu", false));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}



