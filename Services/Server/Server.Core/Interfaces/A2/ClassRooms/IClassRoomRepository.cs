using AMMS.Core.Repositories;
using Server.Core.Entities.A2;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.ClassRooms;

public interface IClassRoomRepository : IAsyncRepository<ClassRoom>
{

    Task<Result<ClassRoom>> SaveAsync(ClassRoom data);
    Task<Result<ClassRoom>> SaveDataAsync(ClassRoom data);
}



