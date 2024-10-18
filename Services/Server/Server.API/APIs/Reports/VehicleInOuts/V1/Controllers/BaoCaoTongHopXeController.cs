using AutoMapper;
using ClosedXML.Report;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Interfaces.GIO.VehicleInOuts;
using Server.Core.Interfaces.ScheduleSendEmails.Requests;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Reports.VehicleInOuts.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class BaoCaoTongHopXeController : AuthBaseAPIController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IGIOVehicleInOutRepository _vehicleInOut;

    public BaoCaoTongHopXeController(
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
    [HttpPost("Post")]
    public async Task<ActionResult> Post(VehicleInOutReportRequest request)
    {
        try
        {
            var data = await _vehicleInOut.GetAlls(request);
            if (data.Succeeded)
            {

                var items = data.Data;
                var result = items.GroupBy(o => new { o.DateTimeIn.Value.Date, o.LaneInId }).Select(o => new
                {
                    DateTimeIn = o.FirstOrDefault().DateTimeIn,
                    LaneInName = o.FirstOrDefault().LaneInName,
                    VehicleInOutStatus = o.FirstOrDefault().VehicleInOutStatus,
                    VehicleInOutStatusName = o.FirstOrDefault().VehicleInOutStatusName,
                    Number = o.Count(),
                }).ToList();
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

    /// <summary>
    /// Export
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Export")]
    public async Task<ActionResult> Export(VehicleInOutReportRequest request)
    {
        try
        {
            var items = await _vehicleInOut.GetAlls(request);
            if (items.Succeeded)
            {
                var data = items.Data;
                var result = data.GroupBy(o => new { o.DateTimeIn.Value.Date, o.LaneInId }).Select(o => new
                {
                    DateTimeIn = o.FirstOrDefault().DateTimeIn,
                    LaneInName = o.FirstOrDefault().LaneInName,
                    VehicleInOutStatus = o.FirstOrDefault().VehicleInOutStatus,
                    VehicleInOutStatusName = o.FirstOrDefault().VehicleInOutStatusName,
                    Number = o.Count(),
                }).ToList();


                string companyName = "";
                string diachi = "";

                string inputFileName = "BaoCaoTongHopXe.xlsx";
                string outputFileName = "BaoCaoTongHopXe_" + DateTime.Now.ToString("ddMMyyyy_hhmmstt") + ".xlsx";

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
                        Items = result,
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



