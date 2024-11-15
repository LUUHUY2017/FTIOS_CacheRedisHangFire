using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Application.CronJobs.Params;
using Server.Application.MasterDatas.A2.MonitorDevices.V1.Models;
using Shared.Core.Loggers;

namespace Server.Application.CronJobs;

public partial class CronJobService : ICronJobService
{
    #region Kiểm tra nhận dữ liệu thiết bị
    static bool Is_CheckDataReception { get; set; } = false;
    public async Task CheckDataReception()
    {
        if (Is_CheckDataReception)
            return;

        Is_CheckDataReception = true;
        try
        {
            if (!DeviceParam.DataReception)
            {
                await _monitorDeviceService.OffAllDevice();
                var dataOff = await _dbContext.Device.Where(x => x.Actived == true).ToListAsync();
                var requests = new List<MDeviceStatusRequest>();
                foreach (var device in dataOff)
                {
                    requests.Add(new MDeviceStatusRequest()
                    {
                        serialNumber = device.SerialNumber,
                        connectionStatus = device.ConnectionStatus,
                        connectUpdateTime = device.CheckConnectTime,
                        time_offline = device.DisConnectUpdateTime,
                        time_online = device.ConnectUpdateTime
                    });
                }

                if (_signalRService != null && _signalRService.Connection != null && _signalRService.Connection.State == Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected)
                {
                    await _signalRService.Connection.InvokeAsync("RefreshDevice", JsonConvert.SerializeObject(requests));
                }
            }
            DeviceParam.DataReception = false;
        }
        catch (Exception ex) 
        {
            Logger.Error(ex);
        }
        finally
        {
            Is_CheckDataReception = false;
        }

    }
    #endregion

}

