using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.Devices.Requests;
public class DeviceFilterRequest : BaseFilterRequest
{
    public string? OrganizationId { get; set; } = null;
    public string? DeviceModel { get; set; } = null;
}
