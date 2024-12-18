﻿using Server.Core.Interfaces.TimeAttendenceEvents.Requests;
using Server.Core.Interfaces.TimeAttendenceEvents.Responses;
using Shared.Core.Commons;

namespace Server.Application.MasterDatas.TA.TimeAttendenceEvents.V1;
public partial class TimeAttendenceEventService
{
    public async Task<IQueryable<AttendenceEventReportRes>> GetAlls(AttendenceReportFilterReq request)
    {
        try
        {
            var _data = (from _do in _dbContext.TimeAttendenceEvent
                         join _la in _dbContext.Student on _do.StudentCode equals _la.StudentCode into K
                         from la in K.DefaultIfEmpty()

                         join _dv in _dbContext.Device on _do.DeviceId equals _dv.Id into KD
                         from dv in KD.DefaultIfEmpty()

                         join _or in _dbContext.Organization on la.OrganizationId equals _or.Id into OG
                         from or in OG.DefaultIfEmpty()

                         where
                              (request.StartDate != null ? _do.EventTime >= request.StartDate : true)
                             && (request.EndDate != null ? _do.EventTime <= request.EndDate.Value.Date.AddDays(1).AddMilliseconds(-1) : true)
                            && ((!string.IsNullOrWhiteSpace(request.OrganizationId) && request.OrganizationId != "0") ? _do.OrganizationId == request.OrganizationId : true)

                         orderby _do.EventTime descending
                         select new AttendenceEventReportRes()
                         {
                             Id = _do.Id,
                             Actived = _do.Actived,
                             CreatedDate = _do.CreatedDate,
                             LastModifiedDate = _do.LastModifiedDate,

                             StudentCode = _do.StudentCode,
                             StudentName = la != null ? la.FullName : "",
                             Name = la != null ? la.Name : "",
                             ClassName = la != null ? la.ClassName : "",
                             DateOfBirth = la != null ? la.DateOfBirth : "",

                             DeviceId = _do.DeviceId,
                             DeviceIP = _do.DeviceIP,
                             DeviceName = dv != null ? dv.DeviceName : "",

                             OrganizationId = la != null ? la.OrganizationId : null,

                             OrganizationCode = or != null ? or.OrganizationCode : "",
                             OrganizationName = or != null ? or.OrganizationName : "",

                             Description = _do.Description,

                             EnrollNumber = _do.EnrollNumber,
                             EventTime = _do.EventTime,
                             EventType = _do.EventType,
                             FormSendSMS = _do.FormSendSMS,

                             GetMode = _do.GetMode,
                             InOutMode = _do.InOutMode,
                             LastModifiedBy = _do.LastModifiedBy,
                             Logs = _do.Logs,
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

                             ValueAbSent = _do.ValueAbSent,
                             IsLate = _do.IsLate,
                             LateTime = _do.LateTime,
                             IsOffSoon = _do.IsOffSoon,
                             OffSoonTime = _do.OffSoonTime,
                         });

            return _data;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<IQueryable<AttendenceEventReportRes>> ApplyFilter(IQueryable<AttendenceEventReportRes> query, List<FilterItems>? filters)
    {
        if (filters == null && filters.Count == 0)
            return query;

        foreach (var filter in filters)
        {
            switch (filter.PropertyName.ToLower())
            {
                case "studentname":
                    if (filter.Comparison == 0)
                        query = query.Where(p => p.StudentName.Contains(filter.Value.Trim()));
                    break;
                case "studentcode":
                    if (filter.Comparison == 0)
                        query = query.Where(p => p.StudentCode.Contains(filter.Value.Trim()));
                    break;

                case "devicename":
                    if (filter.Comparison == 0)
                        query = query.Where(p => p.DeviceName.Contains(filter.Value.Trim()));
                    break;


                case "attendencesection":
                    if (!string.IsNullOrWhiteSpace(filter.Value))
                    {
                        int section = int.Parse(filter.Value);
                        query = query.Where(p => p.AttendenceSection == section);
                    }
                    break;

                case "eventtype":
                    if (!string.IsNullOrWhiteSpace(filter.Value))
                    {
                        var statusValue = filter.Value.ToLower();
                        if (statusValue == "true")
                            query = query.Where(p => p.EventType == true);
                        else
                            query = query.Where(p => p.EventType == null || p.EventType == false);
                    }
                    break;

                case "valueabsent":
                    if (!string.IsNullOrWhiteSpace(filter.Value))
                    {
                        query = query.Where(p => p.ValueAbSent == filter.Value);
                    }
                    break;
                case "classname":
                    if (filter.Comparison == 0)
                        query = query.Where(p => p.ClassName.Contains(filter.Value.Trim()));
                    break;
                case "organizationname":
                    if (filter.Comparison == 0)
                        query = query.Where(p => p.OrganizationName.Contains(filter.Value.Trim()));
                    break;

                case "deviceid":
                    if (filter.Comparison == 0)
                        query = query.Where(p => p.DeviceId.Contains(filter.Value.Trim()));
                    break;

                default:
                    break;
            }
        }
        return query;
    }
}

