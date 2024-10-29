using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Devices.RequeResponsessts;

namespace Server.Application.MasterDatas.A2.Devices.Models;

public class DeviceSelectModel
{
    public List<A2_Device> DeviceSelected { get; set; }
    public List<A2_Device> DeviceUnSelected { get; set; }
}
