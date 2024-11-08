using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Lanes.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.Devices;

public interface ILaneRepository
{
    Task<Result<Lane>> GetById(string id);
    Task<List<Lane>> Gets(LaneFilterRequest request);
    Task<List<Lane>> GetAll();
    Task<Result<Lane>> UpdateAsync(Lane data);
    Task<Result<Lane>> ActiveAsync(ActiveRequest data);
    Task<Result<Lane>> InActiveAsync(InactiveRequest data);



}



