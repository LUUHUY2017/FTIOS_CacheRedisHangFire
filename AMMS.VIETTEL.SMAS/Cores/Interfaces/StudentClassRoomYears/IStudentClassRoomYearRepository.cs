using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using Shared.Core.Commons;
using Shared.Core.Repositories;

namespace AMMS.VIETTEL.SMAS.Cores.Interfaces.StudentClassRoomYears;

public interface IStudentClassRoomYearRepository : IAsyncRepository<StudentClassRoomYear>
{

    Task<Result<StudentClassRoomYear>> SaveAsync(StudentClassRoomYear data);
    Task<Result<StudentClassRoomYear>> SaveDataAsync(StudentClassRoomYear data);
}



