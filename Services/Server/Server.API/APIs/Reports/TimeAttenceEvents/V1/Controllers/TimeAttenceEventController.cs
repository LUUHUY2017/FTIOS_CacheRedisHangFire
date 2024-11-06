using AutoMapper;
using ClosedXML.Report;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.TA.TimeAttendenceEvents.V1;
using Server.Core.Interfaces.GIO.VehicleInOuts;
using Server.Core.Interfaces.TimeAttendenceEvents.Requests;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Reports.TimeAttenceEvents.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TimeAttenceEventController : AuthBaseAPIController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IGIOVehicleInOutRepository _vehicleInOut;
    private readonly TimeAttendenceEventService _timeService;

    public TimeAttenceEventController(
        IMediator mediator,
        IMapper mapper,
        IGIOVehicleInOutRepository vehicleInOut,
        TimeAttendenceEventService timeService
        )
    {
        _mediator = mediator;
        _mapper = mapper;
        _vehicleInOut = vehicleInOut;
        _timeService = timeService;

    }

    /// <summary>
    /// Post
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Post")]
    public async Task<ActionResult> Post(AttendenceReportFilterReq request)
    {
        try
        {
            var data = await _timeService.GetAlls(request);
            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Export
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Export")]
    public async Task<ActionResult> Export(AttendenceReportFilterReq request)
    {
        try
        {
            var retval = await _timeService.GetAlls(request);
            if (!retval.Succeeded)
                return NoContent();
            var items = retval.Data;

            string companyName = "";
            string diachi = "";

            string inputFileName = "LICHSUDIEMDANH.xlsx";
            string outputFileName = "LICHSUDIEMDANH_" + DateTime.Now.ToString("ddMMyyyy_hhmmstt") + ".xlsx";

            var rootParth = Common.GetExcelFolder();
            var rootParth_Output = Common.GetExcelDateFullFolder(DateTime.Now.Date);
            var parth_Output = Common.GetExcelDatePathFolder(DateTime.Now.Date);

            string inputFile = rootParth + inputFileName;
            string outputFile = rootParth_Output + outputFileName;

            //var company = _GIO_PersonInOutService.GetA0Organization();
            //if (company != null)
            //{
            //    companyName = company.OrganizationName.ToUpper();
            //    diachi = company.OrganizationAddress;
            //}
            using (var template = new XLTemplate(inputFile))
            {
                var data1 = new
                {
                    TuNgay = request.StartDate.Value.ToString("dd/MM/yyyy"),
                    DenNgay = request.EndDate.Value.ToString("dd/MM/yyyy"),
                    ThoiGianIn = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                    Items = items,
                    CompanyName = companyName,
                    Address = diachi,
                };
                template.AddVariable(data1);
                var start = DateTime.Now;
                template.Generate();
                template.SaveAs(outputFile);
            }
            return Ok(parth_Output + outputFileName);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// GetImage
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("GetImage")]
    public async Task<ActionResult> GetImage(string id)
    {
        try
        {
            var items = await _vehicleInOut.GetImage(id);
            return Ok(items);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}



