using AMMS.Core.Repositories;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ClassRooms.Reponses;
using Server.Core.Interfaces.A2.ClassRooms.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.ClassRooms;

public interface IClassRoomRepository : IAsyncRepository<ClassRoom>
{

    Task<Result<ClassRoom>> SaveAsync(ClassRoom data);
    Task<Result<ClassRoom>> SaveDataAsync(ClassRoom data);

    Task<Result<ClassRoom>> GetById(string id);
    Task<List<ClassRoomReportResponse>> GetAlls(ClassRoomFilterRequest request);
    Task<List<ClassRoom>> Gets(bool actived = true);
    Task<Result<ClassRoom>> UpdateAsync(ClassRoom data);
    Task<Result<ClassRoom>> ActiveAsync(ActiveRequest data);
    Task<Result<ClassRoom>> InActiveAsync(InactiveRequest data);
}



