using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.SyncDeviceServers.Requests;

public class SyncDeviceServerFilterReq : BaseReportRequest
{
    public string? ClassId { get; set; }
    public string? DeviceId { get; set; }
}

