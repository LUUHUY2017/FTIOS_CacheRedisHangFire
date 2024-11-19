using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using Shared.Core.Commons;
using Shared.Core.Repositories;

namespace AMMS.VIETTEL.SMAS.Cores.Interfaces.ClassRooms;

public interface IClassRoomRepository : IAsyncRepository<ClassRoom>
{

    Task<Result<ClassRoom>> SaveAsync(ClassRoom data);
    Task<Result<ClassRoom>> SaveDataAsync(ClassRoom data);
}



