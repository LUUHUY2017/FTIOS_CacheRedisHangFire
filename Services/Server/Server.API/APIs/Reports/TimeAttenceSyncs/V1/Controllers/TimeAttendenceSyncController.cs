using AMMS.Share.WebApp.Helps;
using AutoMapper;
using ClosedXML.Report;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Application.MasterDatas.TA.TimeAttendenceSyncs.V1;
using Server.Core.Interfaces.GIO.VehicleInOuts;
using Server.Core.Interfaces.TimeAttendenceEvents.Requests;
using Server.Core.Interfaces.TimeAttendenceSyncs.Responses;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Reports.TimeAttenceSyncs.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize("Bearer")]
[AuthorizeMaster]
public class TimeAttendenceSyncController : AuthBaseAPIController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IGIOVehicleInOutRepository _vehicleInOut;
    private readonly TimeAttendenceSyncService _timeService;

    public TimeAttendenceSyncController(
        IMediator mediator,
        IMapper mapper,
        TimeAttendenceSyncService timeService
        )
    {
        _mediator = mediator;
        _mapper = mapper;
        _timeService = timeService;

    }

    /// <summary>
    /// Post
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Post")]
    public async Task<ActionResult> Post(AttendenceSyncReportFilterReq request)
    {
        try
        {
            request.OrganizationId = GetOrganizationId();
            var items = await _timeService.GetAlls(request);
            if (request.FilterItems != null && request.FilterItems.Count > 0)
            {
                foreach (var filter in request.FilterItems)
                {
                    items = await _timeService.ApplyFilter(items, filter);
                }
            }

            int totalRow = await items.Select(o => o.Id).CountAsync();
            // phân trang
            int skip = (request.CurentPage.Value - 1) * (request.RowsPerPage.Value);
            int totalPage = 0;
            totalPage = totalRow / (request.RowsPerPage.Value);
            if (totalRow % (request.RowsPerPage.Value) > 0)
                totalPage++;

            var datas = await items.Skip(skip).Take(request.RowsPerPage.Value).ToListAsync();
            int totalDataRow = datas.Count();

            var retVal = new
            {
                items = datas,

                totalPage = totalPage,
                totalRow = totalRow,
                totalDataRow = totalDataRow,

                rowPerPage = request.RowsPerPage,
                curentPage = request.CurentPage,
            };
            return Ok(new Result<object>(retVal, "Thành công!", true));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// PostGeneral
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("PostGeneral")]
    public async Task<ActionResult> PostGeneral(AttendenceSyncReportFilterReq request)
    {
        try
        {
            request.OrganizationId = GetOrganizationId();
            var items = await _timeService.GetAlls(request);

            int totalAmount = await items.Select(o => new { o.TimeAttendenceEventId }).CountAsync();
            int totalFace = await items.Select(o => new { o.TimeAttendenceEventId, o.SyncStatus }).Distinct().CountAsync(o => o.SyncStatus != null);
            int totalCurrent = totalAmount - totalFace;

            var retVal = new
            {
                totalAmount = totalAmount,
                totalFace = totalFace,
                totalCurrent = totalCurrent,
            };
            return Ok(new Result<object>(retVal, "Thành công", true));
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
    public async Task<ActionResult> Export(AttendenceSyncReportFilterReq request)
    {
        try
        {
            request.OrganizationId = GetOrganizationId();
            var datas = await _timeService.GetAlls(request);
            if (request.FilterItems != null && request.FilterItems.Count > 0)
            {
                foreach (var filter in request.FilterItems)
                {
                    datas = await _timeService.ApplyFilter(datas, filter);
                }
            }

            var items = await datas.ToListAsync();

            string companyName = "";
            string diachi = "";

            string inputFileName = "LICHSUDONGBODIEMDANH.xlsx";
            string outputFileName = "LICHSUDONGBODIEMDANH_" + DateTime.Now.ToString("ddMMyyyy_hhmmstt") + ".xlsx";

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

    /// <summary>
    /// ĐỒng bộ lại 1 bản ghi
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("PostSyncItem")]
    public async Task<IActionResult> PostSyncItem(AttendenceSyncReportRes request)
    {
        try
        {
            var retval = await _timeService.SyncAttendenceToSmas(request.TimeAttendenceEventId);
            return Ok(retval);
        }
        catch (Exception ex)
        {
            return Ok(new Result<object>("Lỗi:" + ex.Message, false));
        }
    }

    /// <summary>
    /// Chạy lại bản tin bị misss
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("PostAgain")]
    public async Task<IActionResult> PostAgain(AttendenceSyncReportFilterReq request)
    {
        try
        {
            var retval = await _timeService.PostAgain(request);
            return Ok(retval);
        }
        catch (Exception ex)
        {
            return Ok(new Result<object>("Lỗi:" + ex.Message, false));
        }
    }

}



