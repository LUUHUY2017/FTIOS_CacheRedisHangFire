using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Lanes.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.Devices;

public interface ILaneRepository
{
    Task<Result<A2_Lane>> GetById(string id);
    Task<List<A2_Lane>> Gets(LaneFilterRequest request);
    Task<List<A2_Lane>> GetAll();
    Task<Result<A2_Lane>> UpdateAsync(A2_Lane data);
    Task<Result<A2_Lane>> ActiveAsync(ActiveRequest data);
    Task<Result<A2_Lane>> InActiveAsync(InactiveRequest data);



}



