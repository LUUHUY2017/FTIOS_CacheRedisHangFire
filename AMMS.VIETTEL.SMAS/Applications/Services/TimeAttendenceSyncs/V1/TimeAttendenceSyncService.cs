using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart;
using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart.Responses;
using AMMS.VIETTEL.SMAS.Cores.Entities.TA;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using EventBus.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;
using Shared.Core.Loggers;
using Shared.Core.SignalRs;

namespace AMMS.VIETTEL.SMAS.Applications.Services.TimeAttendenceSyncs;
public partial class TimeAttendenceSyncService
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly ISignalRClientService _signalRClientService;
    private readonly IConfiguration _configuration;
    private readonly ViettelDbContext _dbContext;
    private readonly SmartService _smartService;


    public TimeAttendenceSyncService(
        IBus bus,
        IConfiguration configuration,
        IEventBusAdapter eventBusAdapter,
        ISignalRClientService signalRClientService,
        ViettelDbContext dbContext,
        SmartService smartService
        )
    {
        _configuration = configuration;
        _eventBusAdapter = eventBusAdapter;
        _signalRClientService = signalRClientService;
        _dbContext = dbContext;
        _smartService = smartService;
    }

    public async Task<TimeAttendenceSync> GetByEventIdAsync(string id)
    {
        try
        {
            var _data = await (from _do in _dbContext.TimeAttendenceSync where _do.TimeAttendenceEventId == id select _do).OrderByDescending(o => o.CreatedDate).FirstOrDefaultAsync();
            return _data;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<Result<TimeAttendenceEvent>> ProcessAtendenceData(SyncDataRequest data)
    {
        try
        {
            var res = await _smartService.PostSyncAttendence2Smas(data, data.schoolCode);
            if (res == null || !res.IsSuccess)
                return new Result<TimeAttendenceEvent>($"Lỗi đồng bộ: Đồng bộ không thành công", false);
            try
            {
                var _listLog = new List<TimeAttendenceSync>();
                foreach (var ite in res.Responses)
                {
                    var sync = await GetByEventIdAsync(data.id);
                    if (sync != null)
                    {
                        if (sync.SyncStatus != true)
                            sync.SyncStatus = ite.status;

                        sync.Message += $"[{DateTime.Now:dd/MM/yy HH:mm:ss}]: {ite.message}\r\n";
                        sync.LastModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        var log = new TimeAttendenceSync()
                        {
                            TimeAttendenceEventId = data.id,
                            SyncStatus = ite.status,
                            Message = $"[{DateTime.Now:dd/MM/yy HH:mm:ss}]: {ite.message}\r\n",
                            CreatedDate = DateTime.Now,
                            LastModifiedDate = DateTime.Now,
                        };
                        await _dbContext.TimeAttendenceSync.AddAsync(log);
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

