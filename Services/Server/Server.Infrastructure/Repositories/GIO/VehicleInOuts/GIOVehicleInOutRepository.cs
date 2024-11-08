using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.GIO;
using Server.Core.Interfaces.GIO.VehicleInOuts;
using Server.Core.Interfaces.GIO.VehicleInOuts.Responses;
using Server.Core.Interfaces.ScheduleSendEmails.Requests;
using Server.Core.Interfaces.ScheduleSendEmails.Responses;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.GIO.VehicleInOuts;

public class GIOVehicleInOutRepository : IGIOVehicleInOutRepository
{
    private readonly IMasterDataDbContext _db;
    public GIOVehicleInOutRepository(IMasterDataDbContext biDbContext)
    {
        _db = biDbContext;
    }

    public async Task<Result<List<VehicleInOutReportResponse>>> GetAlls(VehicleInOutReportRequest request)
    {
        try
        {
            var _data = await (from _do in _db.VehicleInOut
                               join _la in _db.Lane on _do.LaneInId equals _la.Id into K
                               from la in K.DefaultIfEmpty()
                               where
                                (request.StartDate != null ? _do.CreatedDate.Date >= request.StartDate.Value.Date : true)
                                && (request.EndDate != null ? _do.CreatedDate.Date <= request.EndDate.Value.Date : true)
                                && (!string.IsNullOrWhiteSpace(request.OrganizationId) ? _do.OrganizationId == request.OrganizationId : true)
                                && (!string.IsNullOrWhiteSpace(request.LaneId) ? _do.LaneInId == request.LaneId : true)
                                //&& (request.VehicleInOutStatus != 0 ? _do.VehicleInOutStatus == request.VehicleInOutStatus : true) 
                                && (!string.IsNullOrWhiteSpace(request.PlateNumber) ? _do.PlateNumber.Contains(request.PlateNumber) : true)

                               orderby _do.CreatedDate descending
                               select new VehicleInOutReportResponse()
                               {
                                   Id = _do.Id,
                                   Actived = _do.Actived,
                                   CreatedDate = _do.CreatedDate,
                                   LastModifiedDate = _do.LastModifiedDate,
                                   DateTimeIn = _do.DateTimeIn,
                                   DateTimeOut = _do.DateTimeOut,
                                   PlateNumber = _do.PlateNumber,
                                   VehicleCode = _do.VehicleCode,
                                   VehicleInOutStatus = _do.VehicleInOutStatus,
                                   VehicleName = _do.VehicleName,
                                   VehicleNoInDay = _do.VehicleNoInDay,
                                   PlateNumberIdentification = _do.PlateNumberIdentification,
                                   PlateNumberIdentificationOut = _do.PlateNumberIdentificationOut,
                                   GateInId = _do.GateInId,
                                   GateOutId = _do.GateOutId,
                                   LaneInId = _do.LaneInId,
                                   LaneOutId = _do.LaneOutId,
                                   VehicleId = _do.VehicleId,
                                   Note = _do.Note,
                                   LaneInName = la != null ? la.LaneName : "",
                                   LaneOutName = la != null ? la.LaneName : "",
                                   VehicleInOutStatusName = _do.VehicleInOutStatus == 1 ? "Lượt vào" : "Lượt ra",
                                   Lpr = !string.IsNullOrWhiteSpace(_do.PlateNumber) ? "LPR" : "None LPR",

                                   PlateColor = _do.PlateColor,
                                   VehicleType = _do.VehicleType,
                                   VehicleColor = _do.VehicleColor,
                                   Speed = _do.Speed,
                                   Direction = _do.Direction,
                                   Location = _do.Location,

                               }).ToListAsync();

            return new Result<List<VehicleInOutReportResponse>>(_data, "Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<VehicleInOutReportResponse>>("Lỗi: " + ex.ToString(), false);
        }
    }


    public async Task<Result<ImageViewReportResponse>> GetImage(string id)
    {
        ImageViewReportResponse images = null;
        try
        {
            var data = await _db.Image.Where(o => o.ReferenceId == id).ToListAsync();
            if (data.Count > 0)
            {
                images = new ImageViewReportResponse()
                {

                    bien_so_xe_sau_lan1 = data.FirstOrDefault(i => i.ImageIndex == 1 && i.IsPlateNumberImage.Value == true && i.TimeWeight == 1),
                    anh_chup_truoc_lan1 = data.FirstOrDefault(i => i.ImageIndex == 1 && i.IsPlateNumberImage.Value == false && i.TimeWeight == 1),
                    anh_chup_sau_lan1 = data.FirstOrDefault(i => i.ImageIndex == 0 && i.IsPlateNumberImage.Value == false && i.TimeWeight == 1),

                    bien_so_xe_sau_lan2 = data.FirstOrDefault(i => i.ImageIndex == 1 && i.IsPlateNumberImage.Value == true && i.TimeWeight == 2),
                    anh_chup_truoc_lan2 = data.FirstOrDefault(i => i.ImageIndex == 1 && i.IsPlateNumberImage.Value == false && i.TimeWeight == 2),
                    anh_chup_sau_lan2 = data.FirstOrDefault(i => i.ImageIndex == 0 && i.IsPlateNumberImage.Value == false && i.TimeWeight == 2),

                    //bien_so_xe_truoc_lan1 = data.FirstOrDefault(i => i.ImageIndex == 0 && i.IsPlateNumberImage.Value == true && i.TimeWeight == 1),
                    //anh_toancanh_lan1 = data.FirstOrDefault(i => i.ImageIndex == 2 && i.TimeWeight == 1),
                    //bien_so_xe_truoc_lan2 = data.FirstOrDefault(i => i.ImageIndex == 0 && i.IsPlateNumberImage.Value == true && i.TimeWeight == 2),
                    //anh_toancanh_lan2 = data.FirstOrDefault(i => i.ImageIndex == 2 && i.TimeWeight == 2)
                };
                return new Result<ImageViewReportResponse>(images, "Thành công", true);
            }
            return new Result<ImageViewReportResponse>("Không tìm thấy thông tin", false);

        }
        catch (Exception ex) { return new Result<ImageViewReportResponse>("Lỗi: " + ex.ToString(), false); }
    }
    public async Task<Result<VehicleInOut>> CreateAsync(VehicleInOut data)
    {
        string message = "";
        try
        {
            var _order = new VehicleInOut();
            data.CopyPropertiesTo(_order);
            _db.VehicleInOut.Add(_order);

            message = "Thêm mới thành công";
            var retVal = await _db.SaveChangesAsync();
            return new Result<VehicleInOut>(data, message, true);
        }
        catch (Exception ex)
        {
            return new Result<VehicleInOut>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<VehicleInOut>> UpdateAsync(VehicleInOut data)
    {
        string message = "";
        try
        {
            var _order = _db.VehicleInOut.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.VehicleInOut.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new VehicleInOut();
                data.CopyPropertiesTo(_order);
                _db.VehicleInOut.Add(_order);
                message = "Thêm mới thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<VehicleInOut>(data, message, true);
        }
        catch (Exception ex)
        {
            return new Result<VehicleInOut>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<int>> DeleteAsync(string id)
    {
        try
        {
            var result = _db.VehicleInOut.FirstOrDefault(o => o.Id == id);
            if (result == null)
                return new Result<int>("Không tìm thấy dữ liệu", false);

            _db.VehicleInOut.Remove(result);
            var retVal = await _db.SaveChangesAsync();

            return new Result<int>(retVal, "Xóa thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<int>(0, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<List<RangeHour>> GetRangHour(int startHour, int endHour)
    {
        try
        {
            var retval = new List<RangeHour>();
            while (startHour <= endHour)
            {

                string timePeriod = startHour.ToString() + ":00 AM";
                if (startHour > 12)
                {
                    timePeriod = endHour.ToString() + ":00 PM";
                }

                retval.Add(new RangeHour()
                {
                    Id = startHour,
                    TimePeriod = timePeriod,
                });
                startHour = startHour++;
            }
            return retval;
        }
        catch (Exception ex)
        {
            return new List<RangeHour>();
        }
    }




}