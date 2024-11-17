using Server.Core.Entities.A0;

namespace Server.Core.Interfaces.A2.SyncDeviceServers.Responses;

public class SyncDeviceServerReportRes : PersonSynToDevice
{
    public string StudentCode { get; set; }
    public string StudentName { get; set; }
    public string Name { get; set; }
    public string ClassName { get; set; }

    public string IPAddress { get; set; }
    public string DeviceName { get; set; }


}


