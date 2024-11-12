using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Core.Loggers;

namespace AMMS.ZkAutoPush.Applications.CronJobs;

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
            Logger.Warning(DateTime.Now.ToString("HH:mm:ss dd/MM/yyy") + ": Check device online");


            var terminals = await _dbContext.zk_terminal.ToListAsync();


            var terminals_cache = await _deviceCacheService.Gets();


            var terminal_status = new List<terminal_status>();
            if (terminals_cache != null && terminals_cache.Count() > 0)
            {
                foreach (var d in terminals_cache)
                {
                    var td = new terminal_status();
                    var datetimeNow = DateTime.Now;

                    d.last_checkconnection = datetimeNow;

                    //Xử lý kiểm tra offline
                    if (d.online_status == true)
                    {
                        if (d.last_activity == null)
                        {
                            d.time_offline = datetimeNow;
                            d.online_status = false;
                        }
                        else if ((datetimeNow - d.last_activity.Value).TotalMinutes > 5)
                        {
                            d.time_offline = datetimeNow;
                            d.online_status = false;
                        }
                    }

                    td.connectUpdateTime = datetimeNow;
                    td.serialNumber = d.sn;
                    td.connectionStatus = d.online_status ?? false;
                    td.time_offline = d.time_offline;
                    td.time_online = d.time_online;
                    terminal_status.Add(td);

                    //Lưu vào csdl
                    var terminal = terminals.FirstOrDefault(o => o.sn == d.sn);
                    if (terminal != null)
                    {
                        terminal.last_activity = d.last_activity;
                        terminal.last_checkconnection = d.last_checkconnection;
                        terminal.time_offline = d.time_offline;
                        terminal.time_offline = d.time_online;
                        terminal.online_status = d.online_status;
                        terminal.change_time = d.change_time;
                        _dbContext.zk_terminal.Update(terminal);
                    }
                    // Lưu vào caches
                    await _deviceCacheService.Save(d);
                }
                await _dbContext.SaveChangesAsync();
            }
            //Đẩy trạng thái qua signal r
            try
            {
                Logger.Warning(DateTime.Now.ToString("HH:mm:ss dd/MM/yyy") + ": Gửi qua sinalr");

                //Gửi qua sinalr
                if (terminal_status != null && terminal_status.Count() > 0)
                {
                    if (_signalRService != null && _signalRService.Connection != null && _signalRService.Connection.State == HubConnectionState.Connected)
                    {
                        await _signalRService.Connection.InvokeAsync("RefreshDevice", JsonConvert.SerializeObject(terminal_status));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
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
    /// <summary>
    /// Trạng thái thiết bị
    /// </summary>
    public class terminal_status
    {
        /// <summary>
        /// Serrial number của thiết bị
        /// </summary>
        public string serialNumber { get; set; } = "";
        /// <summary>
        /// Trạng thái thiết bị
        /// </summary>
        public bool connectionStatus { get; set; } = false;
        /// <summary>
        /// Thời gian kiểm tra
        /// </summary>
        public DateTime connectUpdateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// Thời gian kết nối
        /// </summary>
        public DateTime? time_online { get; set; }
        /// <summary>
        /// Thời gian mất kết nôi
        /// </summary>
        public DateTime? time_offline { get; set; }

    }

    #endregion
}
