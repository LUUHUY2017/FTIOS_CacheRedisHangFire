using AMMS.DeviceData.RabbitMq;
using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart;
using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart.Responses;
using AMMS.VIETTEL.SMAS.Cores.Entities.TA;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using EventBus.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            Logger.Warning("SMAS_Req:" + JsonConvert.SerializeObject(data));
            var res = await _smartService.PostSyncAttendence2Smas(data, data.schoolCode);
            if (res == null || !res.IsSuccess)
            {
                Logger.Warning("SMAS_Res:" + JsonConvert.SerializeObject(res));
                return new Result<TimeAttendenceEvent>($"Lỗi đồng bộ: Đồng bộ không thành công", false);
            }

            #region Lưu lịch sử đồng bộ
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
                    _dbContext.TimeAttendenceSync.Update(sync);
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
                await _dbContext.SaveChangesAsync();
            }
            #endregion

            #region Bắn thông tin sang Server
            RB_DataResponse rB_Response = new RB_DataResponse()
            {
                Id = Guid.NewGuid().ToString(),
                Content = JsonConvert.SerializeObject(res),
                ReponseType = RB_DataResponseType.AttendencePush,
            };
            var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.SMAS_Auto_Push_Server}");
            await aa.Send(rB_Response);
            #endregion


            return new Result<TimeAttendenceEvent>($"Đồng bộ thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<TimeAttendenceEvent>($"Lỗi đồng bộ: " + ex.Message, false);
        }
    }
}

