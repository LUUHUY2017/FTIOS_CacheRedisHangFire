using Shared.Core.Commons;
using Microsoft.AspNetCore.SignalR.Client;

namespace Shared.Core.SignalRs;
public delegate void ShowMessage(string type, string message);


public interface ISignalRClientService
{
    public HubConnection Connection { get; }
    public void Init(string url);
    public void Start();
    public void Stop();

    public bool? IsConnected();
}
public class SignalRClientService : ISignalRClientService
{
    public event ShowMessage OnShowMessage;

    System.Timers.Timer timerCheckSignalR;

    public SignalRClientService()
    {

    }
    public void Init(string url)
    {
        timerCheckSignalR = new System.Timers.Timer();
        timerCheckSignalR.Interval = 10000;
        timerCheckSignalR.Elapsed += TimerCheckSignalR_Tick;

        var timeReconnect = new TimeSpan[1] { new TimeSpan(10000) };
        Connection = new HubConnectionBuilder()
          .WithUrl(url)
          .WithAutomaticReconnect(timeReconnect)
          .Build();


        Connection.HandshakeTimeout = new TimeSpan(0, 0, 5);

        Connection.Closed += async (error) =>
        {
            if (OnShowMessage != null)
                OnShowMessage("Thông báo", "SignalR Closed ");
        };

        Connection.Reconnected += async (error) =>
        {
            if (OnShowMessage != null)
                OnShowMessage("SignalR", " Reconnecting ");
        };
        Connection.Reconnecting += async (error) =>
        {
            connected = false;
            if (OnShowMessage != null)
                OnShowMessage("SignalR", "Reconnecting " + error.Message);
        };
    }
    private async void TimerCheckSignalR_Tick(object sender, EventArgs e)
    {
        timerCheckSignalR.Stop();
        if (Started && Connection.State == HubConnectionState.Disconnected)
            await Connect();
        if (Started)
            timerCheckSignalR.Start();
    }

    public HubConnection Connection { get; private set; }


    public delegate void StartService(bool status);
    public event StartService OnStartService;
    bool _started { get; set; } = false;
    public bool Started
    {
        get
        {
            return _started;
        }
        private set
        {
            if (_started != value)
            {
                _started = value;

                if (OnStartService != null)
                    OnStartService(_started);
            }
            timerCheckSignalR.Enabled = _started;
            if (Started == true)
            {
                if (OnShowMessage != null)
                    OnShowMessage("Thông báo", "Service Started");
            }
            else
            {
                if (OnShowMessage != null)
                    OnShowMessage("Thông báo", "Service Stoped");
            }
        }
    }

    public delegate void ChangeConnect(bool? status);
    public event ChangeConnect OnChangeConnect;
    bool? _connected { get; set; } = false;
    bool? connected
    {
        get
        {
            return _connected;
        }
        set
        {
            if (_connected != value)
            {
                _connected = value;
                if (OnChangeConnect != null)
                    OnChangeConnect(value);

                if (_connected == true)
                {
                    if (OnShowMessage != null)
                        OnShowMessage("Thông báo", "SignalR connected");
                }
                else
                {
                    if (OnShowMessage != null)
                        OnShowMessage("Thông báo", "SignalR lost connect");
                }
            }
        }
    }

    public void Start()
    {
        Started = true;
        try
        {
            Task.Run(Connect);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

    }
    public void Stop()
    {
        Started = false;
        try
        {
            Disconnect();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    async Task Connect()
    {
        try
        {
            await Connection.StopAsync();
        }
        catch (Exception ex)
        {
            if (OnShowMessage != null)
                OnShowMessage("Lỗi", ex.Message);
        }
        try
        {
            await Connection.StartAsync();
            connected = Connection.State == HubConnectionState.Connected;
        }
        catch (Exception ex)
        {
            if (OnShowMessage != null)
                OnShowMessage("Lỗi", ex.Message);
        }

    }
    async void Disconnect()
    {
        try
        {
            await Connection.StopAsync();
            connected = false;
        }
        catch (Exception ex)
        {
            //Console.WriteLine(ex.Message);
            if (OnShowMessage != null)
                OnShowMessage("Lỗi", ex.Message);
        }
    }

    public bool? IsConnected()
    {
        return connected;
    }
}