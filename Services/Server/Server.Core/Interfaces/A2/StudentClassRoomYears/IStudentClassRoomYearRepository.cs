using AMMS.Core.Repositories;
using Server.Core.Entities.A2;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.StudentClassRoomYears;

public interface IStudentClassRoomYearRepository : IAsyncRepository<StudentClassRoomYear>
{

    Task<Result<StudentClassRoomYear>> SaveAsync(StudentClassRoomYear data);
    Task<Result<StudentClassRoomYear>> SaveDataAsync(StudentClassRoomYear data);
}



