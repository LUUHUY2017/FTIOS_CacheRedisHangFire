using Server.Core.Entities.GIO;
using Server.Core.Interfaces.GIO.VehicleInOuts.Responses;
using Server.Core.Interfaces.ScheduleSendEmails.Requests;
using Server.Core.Interfaces.ScheduleSendEmails.Responses;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.GIO.VehicleInOuts;

public interface IGIOVehicleInOutRepository
{
    Task<Result<List<VehicleInOutReportResponse>>> GetAlls(VehicleInOutReportRequest request);
    Task<Result<ImageViewReportResponse>> GetImage(string id);
    Task<Result<GIO_VehicleInOut>> CreateAsync(GIO_VehicleInOut data);
    Task<Result<GIO_VehicleInOut>> UpdateAsync(GIO_VehicleInOut data);
    Task<Result<int>> DeleteAsync(string id);
    Task<List<RangeHour>> GetRangHour(int startHour, int endHour);
}


