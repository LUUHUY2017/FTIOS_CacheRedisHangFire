using AutoMapper;
using ClosedXML.Report;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.API.APIs.Reports.Dashboards.V1.Responses;
using Server.Core.Interfaces.GIO.VehicleInOuts;
using Server.Core.Interfaces.ScheduleSendEmails.Requests;
using Server.Core.Interfaces.ScheduleSendEmails.Responses;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Reports.VehicleInOuts.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class BaoCaoChiTietController : AuthBaseAPIController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IGIOVehicleInOutRepository _vehicleInOut;

    public BaoCaoChiTietController(
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
                int count = data.Data.Count();
                var items = data.Data.Select((_do, index) => new VehicleInOutReportResponse()
                {
                    Id = _do.Id,
                    Actived = _do.Actived,
                    CreatedDate = _do.CreatedDate,
                    DateTimeIn = _do.DateTimeIn,
                    PlateNumber = _do.PlateNumber,
                    VehicleCode = _do.VehicleCode,
                    LaneInId = _do.LaneInId,
                    Note = _do.Note,
                    LaneInName = _do.LaneInName,
                    LaneOutName = _do.LaneOutName,
                    VehicleInOutStatusName = _do.VehicleInOutStatusName,

                    PlateColor = _do.PlateColor,
                    VehicleType = _do.VehicleType,
                    VehicleColor = _do.VehicleColor,
                    Speed = _do.Speed,
                    Direction = _do.Direction,
                    Location = _do.Location,

                    No = count - index,
                }).ToList();
                var rangHour = await _vehicleInOut.GetRangHour(0, 23);





                #region LPR
                var totals = data.Data.GroupBy(o => o.Lpr).Select(g => new ObjectDataDashboard()
                {
                    Name = g.Key,
                    Value = g.Count()
                }).ToList();

                int totalAmount = data.Data.Count();
                if (!data.Data.Any())
                {
                    totals.Add(new ObjectDataDashboard
                    {
                        Name = "LPR",
                        Value = 0
                    });
                    totals.Add(new ObjectDataDashboard
                    {
                        Name = "None LPR",
                        Value = 0
                    });
                }
                else
                {
                    if (totals.Count() == 1)
                    {
                        var lpr = totals.FirstOrDefault(o => o.Name == "LPR");
                        if (lpr == null)
                        {
                            totals.Add(new ObjectDataDashboard
                            {
                                Name = "LPR",
                                Value = 0
                            });
                        }
                        else
                        {
                            totals.Add(new ObjectDataDashboard
                            {
                                Name = "None LPR",
                                Value = 0
                            });
                        }
                    }
                }

                totals = totals.Prepend(new ObjectDataDashboard
                {
                    Name = "Total",
                    Value = totalAmount
                }).ToList();


                #endregion

                #region Type
                var vehicleTypes = data.Data.GroupBy(o => o.VehicleType).Select(g => new ObjectDataDashboard()
                {
                    Name = g.Key ?? "N/A",
                    Value = g.Count(),
                    Percent = ((float)g.Count() / totalAmount) * 100
                }).ToList();
                #endregion

                #region Lane
                var lanes = data.Data.GroupBy(o => o.LaneInName).Select(g => new ObjectDataDashboard()
                {
                    Name = g.Key,
                    Value = g.Count(),
                    Percent = ((float)g.Count() / totalAmount) * 100
                }).ToList();

                #endregion


                var retVal = new
                {
                    items = items,
                    totals = totals,
                    vehicleTypes = vehicleTypes,
                    lanes = lanes,
                };
                return Ok(new Result<object>(retVal, "Thành công!", true));
            }
            else
            {
                var retVal = new
                {
                    items = new List<VehicleInOutReportRequest>(),
                    totals = new List<ObjectDataDashboard>(),
                    vehicleTypes = new List<ObjectDataDashboard>(),
                    lanes = new List<ObjectDataDashboard>(),
                };
                return Ok(new Result<object>(retVal, "Không có dữ liệu  !", true));
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

                int? tongXe = 0;
                string companyName = "";
                string diachi = "";

                string inputFileName = "BaoCaoLuotXe.xlsx";
                string outputFileName = "BaoCaoLuotXe_" + DateTime.Now.ToString("ddMMyyyy_hhmmstt") + ".xlsx";

                var rootParth = Common.GetExcelFolder();
                var rootParth_Output = Common.GetExcelDateFullFolder(DateTime.Now.Date);
                var parth_Output = Common.GetExcelDatePathFolder(DateTime.Now.Date);

                string inputFile = rootParth + inputFileName;
                string outputFile = rootParth_Output + outputFileName;

                tongXe = items.Data.Count();

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
                        TongXe = tongXe,
                        Items = items.Data,
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



