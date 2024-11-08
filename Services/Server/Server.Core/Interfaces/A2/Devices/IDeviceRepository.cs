using AMMS.Core.Repositories;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Devices.RequeResponsessts;
using Server.Core.Interfaces.A2.Devices.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.Devices;

public interface IDeviceRepository : IAsyncRepository<Device>
{
    Task<List<DeviceResponse>> GetFilters(DeviceFilterRequest req);
    Task<Result<Device>> UpdateAsyncV2(Device data);
}



