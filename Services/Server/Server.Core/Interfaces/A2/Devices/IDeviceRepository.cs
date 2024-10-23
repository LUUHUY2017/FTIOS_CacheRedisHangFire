using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Devices.RequeResponsessts;
using Server.Core.Interfaces.A2.Devices.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.Devices;

public interface IDeviceRepository
{
    Task<Result<A2_Device>> GetById(string id);
    Task<List<DeviceResponse>> Gets(DeviceFilterRequest request);
    Task<List<A2_Device>> GetAll();
    Task<Result<A2_Device>> UpdateAsync(A2_Device data);
    Task<Result<A2_Device>> ActiveAsync(ActiveRequest data);
    Task<Result<A2_Device>> InActiveAsync(InactiveRequest data);



}



