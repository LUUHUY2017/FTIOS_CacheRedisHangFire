using Shared.Core.Loggers;

namespace AMMS.Hanet.Applications.CronJobs;

public partial class CronJobService : ICronJobService
{
    #region Kiểm tra thiết bị online/offline
    /// <summary>
    /// Kiểm tra thiết bị online/offline
    /// </summary>

    static bool Is_CheckDeviceOnline { get; set; } = false;
    public async Task CheckDeviceOnline()
    {
        if (Is_CheckDeviceOnline)
            return;

        Is_CheckDeviceOnline = true;
        try
        {
            Console.WriteLine("Check device online");

            //var terminals_cache = await _deviceCacheService.Gets();

            //var terminals = await _biDbContext.Terminals.Where(o => o.Actived == 1).ToListAsync();

            //if (terminals_cache != null && terminals_cache.Count() > 0)
            //{
            //    foreach (var d in terminals_cache)
            //    {
            //        d.LastTimeCheckConnection = DateTime.Now;

            //        if (d.LastTimeUpdateSocket == null || (d.LastTimeCheckConnection - d.LastTimeUpdateSocket).Value.TotalMinutes > 5)
            //        {
            //            if (d.OnlineStatus == "Online" || string.IsNullOrEmpty(d.OnlineStatus))
            //            {
            //                d.OnlineStatus = "Offline";
            //                d.LastTimeOffline = DateTime.Now;

            //                await _biDbContext.TerminalStatusLogs.AddAsync(new TerminalStatusLog()
            //                {
            //                    TerminalId = d.Id,
            //                    CreatedAt = (DateTime)d.LastTimeOffline,
            //                    Status = false,
            //                });
            //                //Đẩy dữ liệu vào Queue
            //                //_eventBusAdapter.GetSendEndpointAsync(EventBus.Messages.Common.EventBusConstants.DeviceOnlineOffline_Queue).Result.Send(d).Wait();
            //                _eventBusAdapter.GetSendEndpointAsync(_configuration.GetValue<string>("DataArea") + EventBus.Messages.Common.EventBusConstants.DeviceOnlineOffline_Queue).Result.Send(d).Wait();
            //            }
            //        }


            //        var terminal = terminals.FirstOrDefault(o => o.SerialNumber == d.SerialNumber);
            //        if (terminal != null)
            //        {
            //            terminal.LastTimeCheckConnection = d.LastTimeCheckConnection;
            //            terminal.LastTimeUpdateSocket = d.LastTimeUpdateSocket;
            //            terminal.LastTimeOffline = d.LastTimeOffline;
            //            terminal.OnlineStatus = d.OnlineStatus;
            //            terminal.WanIpAddress = d.WanIpAddress;
            //            terminal.IpAddress = d.IpAddress;
            //            terminal.HttpPort = d.HttpPort;
            //            terminal.HttpsPort = d.HttpsPort;
            //            _biDbContext.Terminals.Update(terminal);

            //        }
            //    }
            //    await _biDbContext.SaveChangesAsync();
            //}

            //foreach (var terminal in terminals)
            //    await _deviceCacheService.Add(terminal);

            //if (terminals_cache != null && terminals_cache.Count() > 0)
            //{
            //    if (_signalRService != null && _signalRService.Connection != null && _signalRService.Connection.State == Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected)
            //    {
            //        await _signalRService.Connection.InvokeAsync("RefreshDevice", JsonConvert.SerializeObject(terminals));
            //    }
            //}
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        finally
        {
            Is_CheckDeviceOnline = false;
        }
    }

    #endregion
}
