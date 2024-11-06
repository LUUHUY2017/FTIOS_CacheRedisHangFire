namespace Server.Core.Interfaces.A2.SyncDeviceServers.Requests;

public class SyncStudentDeviceReq
{
    public string? Id { get; set; }
    public string? PersonId { get; set; }
    public string? DeviceId { get; set; }

    public string StudentCode { get; set; }
    public string StudentName { get; set; }
}

