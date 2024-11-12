using EventBus.Messages;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Server.Core.Interfaces.TimeAttendenceEvents.Requests;
using Server.Core.Interfaces.TimeAttendenceSyncs.Responses;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;
using Shared.Core.SignalRs;

namespace Server.Application.MasterDatas.TA.TimeAttendenceSyncs.V1;
public partial class TimeAttendenceSyncService
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly ISignalRClientService _signalRClientService;
    private readonly IConfiguration _configuration;
    private readonly IMasterDataDbContext _dbContext;


    public TimeAttendenceSyncService(
        IBus bus,
        IConfiguration configuration,
        IEventBusAdapter eventBusAdapter,
        ISignalRClientService signalRClientService,
        IMasterDataDbContext dbContext
        )
    {
        _configuration = configuration;
        _eventBusAdapter = eventBusAdapter;
        _signalRClientService = signalRClientService;
        _dbContext = dbContext;
    }

    public async Task<IQueryable<AttendenceSyncReportRes>> GetAlls(AttendenceSyncReportFilterReq request)
    {
        try
        {
            var _data = (from _do in _dbContext.TimeAttendenceSync

                         join _la in _dbContext.TimeAttendenceEvent on _do.TimeAttendenceEventId equals _la.Id into K
                         from la in K.DefaultIfEmpty()

                         join _st in _dbContext.Student on la.StudentCode equals _st.StudentCode into KD
                         from st in KD.DefaultIfEmpty()


                         join _or in _dbContext.Organization on la.OrganizationId equals _or.Id into OG
                         from or in OG.DefaultIfEmpty()

                         where
                          (request.StartDate != null ? _do.CreatedDate.Date >= request.StartDate.Value.Date : true)
                          && (request.EndDate != null ? _do.CreatedDate.Date <= request.EndDate.Value.Date : true)
                             && ((!string.IsNullOrWhiteSpace(request.OrganizationId) && request.OrganizationId != "0") ? st.OrganizationId == request.OrganizationId : true)
                          && (!string.IsNullOrWhiteSpace(request.ClassId) ? st.ClassId == request.ClassId : true)

                         orderby _do.CreatedDate descending
                         select new AttendenceSyncReportRes()
                         {
                             Id = _do.Id,
                             Actived = _do.Actived,
                             CreatedDate = _do.CreatedDate,
                             LastModifiedDate = _do.LastModifiedDate,
                             CreatedBy = _do.CreatedBy,
                             ReferenceId = _do.ReferenceId,

                             StudentCode = st != null ? st.StudentCode : "",
                             StudentName = st != null ? st.FullName : "",
                             ClassName = st != null ? st.ClassName : "",

                             OrganizationId = la != null ? la.OrganizationId : null,

                             OrganizationCode = or != null ? or.OrganizationCode : "",
                             OrganizationName = or != null ? or.OrganizationName : "",

                             Message = _do.Message,
                             SyncStatus = _do.SyncStatus,
                             ParamResponses = _do.ParamResponses,
                             ParamRequests = _do.ParamRequests,

                             AttendenceSection = la.AttendenceSection,
                             AbsenceDate = la.AbsenceDate,
                             EventTime = la.EventTime,

                         });

            return _data;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<IQueryable<AttendenceSyncReportRes>> ApplyFilter(IQueryable<AttendenceSyncReportRes> query, FilterItems filter)
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

            case "attendencesection":
                if (!string.IsNullOrWhiteSpace(filter.Value))
                {
                    int section = int.Parse(filter.Value);
                    query = query.Where(p => p.AttendenceSection == section);
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

            case "syncstatus":
                if (!string.IsNullOrWhiteSpace(filter.Value))
                {
                    var statusValue = filter.Value.ToLower();
                    if (statusValue == "true")
                        query = query.Where(p => p.SyncStatus == true);
                    else if (statusValue == "false")
                        query = query.Where(p => p.SyncStatus == false);
                    else
                        query = query.Where(p => p.SyncStatus == null);
                }
                break;

            default:
                break;
        }
        return query;
    }


}

