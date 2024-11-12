using EventBus.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Server.Application.Services.VTSmart;
using Server.Application.Services.VTSmart.Responses;
using Server.Core.Entities.TA;
using Server.Core.Interfaces.TA.TimeAttendenceEvents;
using Server.Core.Interfaces.TA.TimeAttendenceSyncs;
using Server.Core.Interfaces.TimeAttendenceEvents.Requests;
using Server.Core.Interfaces.TimeAttendenceSyncs.Responses;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;
using Shared.Core.Loggers;
using Shared.Core.SignalRs;

namespace Server.Application.MasterDatas.TA.TimeAttendenceSyncs.V1;
public partial class TimeAttendenceSyncService
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly ISignalRClientService _signalRClientService;
    private readonly IConfiguration _configuration;
    private readonly IMasterDataDbContext _dbContext;
    private readonly ITATimeAttendenceSyncRepository _timeAttendenceSyncRepository;
    private readonly ITATimeAttendenceEventRepository _timeAttendenceEventRepository;
    private readonly SmartService _smartService;


    public TimeAttendenceSyncService(
        IBus bus,
        IConfiguration configuration,
        IEventBusAdapter eventBusAdapter,
        ISignalRClientService signalRClientService,
        IMasterDataDbContext dbContext,
        ITATimeAttendenceSyncRepository timeAttendenceSyncRepository,
        ITATimeAttendenceEventRepository timeAttendenceEventRepository,
        SmartService smartService
        )
    {
        _configuration = configuration;
        _eventBusAdapter = eventBusAdapter;
        _signalRClientService = signalRClientService;
        _dbContext = dbContext;
        _smartService = smartService;
        _timeAttendenceSyncRepository = timeAttendenceSyncRepository;
        _timeAttendenceEventRepository = timeAttendenceEventRepository;
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

                         orderby _do.SyncStatus ascending, _do.CreatedDate descending
                         select new AttendenceSyncReportRes()
                         {
                             Id = _do.Id,
                             Actived = _do.Actived,
                             CreatedDate = _do.CreatedDate,
                             LastModifiedDate = _do.LastModifiedDate != null ? _do.LastModifiedDate : _do.CreatedDate,
                             CreatedBy = _do.CreatedBy,
                             ReferenceId = _do.ReferenceId,
                             TimeAttendenceEventId = _do.TimeAttendenceEventId,

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
    public async Task<TimeAttendenceSync> GetByEventIdAsync(string id)
    {
        try
        {
            var _data = await (from _do in _dbContext.TimeAttendenceSync
                               where _do.TimeAttendenceEventId == id
                               select _do).FirstOrDefaultAsync();
            return _data;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<Result<TimeAttendenceEvent>> SyncAttendenceToSmas(string id)
    {
        try
        {
            var item = await _timeAttendenceEventRepository.GetByIdAsync(id);
            if (item == null)
                return new Result<TimeAttendenceEvent>($"Lỗi: Không tìm thấy thông tin điểm danh", false);

            var orgRes = await _dbContext.Organization.FirstOrDefaultAsync(o => o.Id == item.OrganizationId && o.Actived == true);
            if (orgRes == null)
                return new Result<TimeAttendenceEvent>($"Lỗi: Điểm danh thuộc trường học", false);

            var studentAbs = new List<StudentAbsence>();
            ExtraProperties extra = new ExtraProperties()
            {
                absenceTime = item.EventTime
            };
            var el = new StudentAbsence()
            {
                studentCode = item.StudentCode,
                value = item.ValueAbSent,
                extraProperties = extra
            };
            studentAbs.Add(el);

            var req = new SyncDataRequest()
            {
                id = Guid.NewGuid().ToString(),
                schoolCode = orgRes.OrganizationCode,
                absenceDate = DateTime.Now,
                section = 0,
                formSendSMS = 1,
                studentCodeType = 2,
                studentAbsenceByDevices = studentAbs,
            };

            Logger.Warning("SMAS_Req:" + JsonConvert.SerializeObject(req));
            var res = await _smartService.PostSyncAttendence2Smas(req, orgRes.OrganizationCode);
            Logger.Warning("SMAS_Res:" + JsonConvert.SerializeObject(res));

            if (res == null || !res.IsSuccess)
                return new Result<TimeAttendenceEvent>($"Lỗi đồng bộ: Đồng bộ không thành công", false);

            item.EventType = true;

            try
            {
                var _listLog = new List<TimeAttendenceSync>();
                foreach (var ite in res.Responses)
                {
                    var sync = await GetByEventIdAsync(item.Id);
                    if (sync != null)
                    {
                        sync.SyncStatus = ite.status;
                        sync.Message = ite.message;
                        sync.LastModifiedDate = DateTime.Now;
                    }
                }
            }
            catch (Exception ext)
            {
                Logger.Error(ext);
            }

            await _dbContext.SaveChangesAsync();

            return new Result<TimeAttendenceEvent>($"Đồng bộ thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<TimeAttendenceEvent>($"Lỗi đồng bộ: " + ex.Message, false);
        }
    }

}

