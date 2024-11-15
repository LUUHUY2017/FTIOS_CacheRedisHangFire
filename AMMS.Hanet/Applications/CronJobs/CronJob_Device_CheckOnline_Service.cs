using AMMS.Hanet.Applications.V1.Service;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Core.Loggers;
using System.Net;
using System.Net.NetworkInformation;

namespace AMMS.Hanet.Applications.CronJobs;

public partial class CronJobService : ICronJobService
{
    #region Kiểm tra thiết bị online/offline
    /// <summary>
    /// Kiểm tra thiết bị online/offline
    /// </summary>

    static bool Is_CheckDeviceOnline { get; set; } = false;
    static string HanetServer { get; set; } = "https://partner.hanet.ai";
    public async Task CheckDeviceOnline()
    {
        if (Is_CheckDeviceOnline)
            return;

        Is_CheckDeviceOnline = true;
        try
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss dd/MM/yyy") + ": Check device online");
            var connection_status = await CheckServerOnline(HanetServer);

            //Danh sách từ CSDL
            var terminals = await _dbContext.hanet_terminal.ToListAsync();
            //Danh sách từ caches
            var terminals_cache = await _deviceCacheService.Gets();
            //Danh sách trạng thái
            var terminal_status = new List<terminal_status>();
            //Xử lý trạng thái
            if (terminals_cache != null && terminals_cache.Count() > 0)
            {
                foreach (var d in terminals_cache)
                {
                    var td = new terminal_status();
                    var datetimeNow = DateTime.Now;
                    d.last_activity = datetimeNow;
                    d.last_checkconnection = datetimeNow;
                    if (d.online_status == false && connection_status == true)
                    {
                        d.time_online = datetimeNow;
                    }
                    else if (d.online_status == true && connection_status == false)
                    {
                        d.time_offline = datetimeNow;
                    }

                    d.online_status = connection_status;

                    td.connectUpdateTime = datetimeNow;
                    td.serialNumber = d.sn;
                    td.connectionStatus = connection_status;
                    td.time_online = d.time_online;
                    td.time_offline = d.time_offline;

                    terminal_status.Add(td);

                    //Lưu vào csdl
                    var terminal = terminals.FirstOrDefault(o => o.sn == d.sn);
                    if (terminal != null)
                    {
                        terminal.last_checkconnection = d.last_checkconnection;
                        terminal.time_offline = d.time_offline;
                        terminal.time_online = d.time_online;
                        terminal.online_status = d.online_status;
                        terminal.change_time = d.change_time;
                        _dbContext.hanet_terminal.Update(terminal);
                    }
                    // Lưu vào caches
                    await _deviceCacheService.Save(d);
                }
                await _dbContext.SaveChangesAsync();
            }
            //Gửi thông tin qua signalr
            try
            {
                if (terminal_status != null && terminal_status.Count() > 0)
                {
                    await _hanetStartUpService.UpdateStatus(terminal_status);
                }
            }
            catch (Exception)
            {


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
    //Kiem tra có mạng
    public async Task<bool> CheckServerOnline(string server)
    {
        try
        {
            if (string.IsNullOrEmpty(server))
                return false;

            return true;
            WebRequest request = WebRequest.Create(server);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response == null || response.StatusCode != HttpStatusCode.OK)
                    return false;
                else
                    return true;
            }


        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
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
