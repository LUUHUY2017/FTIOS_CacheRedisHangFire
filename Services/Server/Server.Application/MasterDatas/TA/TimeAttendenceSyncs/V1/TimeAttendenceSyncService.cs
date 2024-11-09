using EventBus.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
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

    public async Task<Result<List<AttendenceSyncReportRes>>> GetAlls(AttendenceSyncReportFilterReq request)
    {
        try
        {
            var _data = await (from _do in _dbContext.TimeAttendenceSync

                               join _la in _dbContext.TimeAttendenceEvent on _do.TimeAttendenceEventId equals _la.Id into K
                               from la in K.DefaultIfEmpty()

                               join _st in _dbContext.Student on la.StudentCode equals _st.StudentCode into KD
                               from st in KD.DefaultIfEmpty()

                               where
                                (request.StartDate != null ? _do.CreatedDate.Date >= request.StartDate.Value.Date : true)
                                && (request.EndDate != null ? _do.CreatedDate.Date <= request.EndDate.Value.Date : true)
                                && (!string.IsNullOrWhiteSpace(request.OrganizationId) ? st.OrganizationId == request.OrganizationId : true)
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

                                   Message = _do.Message,
                                   SyncStatus = _do.SyncStatus,
                                   ParamResponses = _do.ParamResponses,
                                   ParamRequests = _do.ParamRequests,

                                   AttendenceSection = la.AttendenceSection,
                                   AbsenceDate = la.AbsenceDate,


                               }).ToListAsync();

            return new Result<List<AttendenceSyncReportRes>>(_data, "Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<AttendenceSyncReportRes>>("Lỗi: " + ex.ToString(), false);
        }
    }



}

