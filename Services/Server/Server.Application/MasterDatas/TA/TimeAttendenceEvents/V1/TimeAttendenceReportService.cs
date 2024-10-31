using Microsoft.EntityFrameworkCore;
using Server.Core.Interfaces.TimeAttendenceEvents.Requests;
using Server.Core.Interfaces.TimeAttendenceEvents.Responses;
using Shared.Core.Commons;

namespace Server.Application.MasterDatas.TA.TimeAttendenceEvents.V1;
public partial class TimeAttendenceEventService
{
    public async Task<Result<List<AttendenceEventReportRes>>> GetAlls(AttendenceReportFilterReq request)
    {
        try
        {
            var _data = await (from _do in _dbContext.TA_TimeAttendenceEvent
                               join _la in _dbContext.A2_Student on _do.StudentCode equals _la.StudentCode into K
                               from la in K.DefaultIfEmpty()

                               where
                                (request.StartDate != null ? _do.CreatedDate.Date >= request.StartDate.Value.Date : true)
                                && (request.EndDate != null ? _do.CreatedDate.Date <= request.EndDate.Value.Date : true)
                                && (!string.IsNullOrWhiteSpace(request.OrganizationId) ? _do.OrganizationId == request.OrganizationId : true)
                               //&& (!string.IsNullOrWhiteSpace(request.LaneId) ? _do.LaneInId == request.LaneId : true)
                               orderby _do.CreatedDate descending
                               select new AttendenceEventReportRes()
                               {
                                   Id = _do.Id,
                                   Actived = _do.Actived,
                                   CreatedDate = _do.CreatedDate,
                                   LastModifiedDate = _do.LastModifiedDate,

                                   StudentCode = _do.StudentCode,
                                   StudentName = la != null ? la.FullName : "",


                                   CreatedBy = _do.CreatedBy,
                                   Description = _do.Description,
                                   DeviceId = _do.DeviceId,
                                   DeviceIP = _do.DeviceIP,

                                   EnrollNumber = _do.EnrollNumber,
                                   EventTime = _do.EventTime,
                                   EventType = _do.EventType,
                                   FormSendSMS = _do.FormSendSMS,

                                   GetMode = _do.GetMode,
                                   InOutMode = _do.InOutMode,
                                   LastModifiedBy = _do.LastModifiedBy,
                                   Logs = _do.Logs,
                                   OrganizationId = _do.OrganizationId,
                                   PersonId = _do.PersonId,
                                   Reason = _do.Reason,
                                   ReferenceId = _do.ReferenceId,

                                   AbsenceDate = _do.AbsenceDate,
                                   AttendenceSection = _do.AttendenceSection,

                                   ClassCode = _do.ClassCode,
                                   SchoolCode = _do.SchoolCode,
                                   SchoolYearCode = _do.SchoolYearCode,
                                   ShiftCode = _do.ShiftCode,

                                   TAMessage = _do.TAMessage,
                                   ValueAbSent = _do.ValueAbSent

                               }).ToListAsync();

            return new Result<List<AttendenceEventReportRes>>(_data, "Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<AttendenceEventReportRes>>("Lỗi: " + ex.ToString(), false);
        }
    }




}

