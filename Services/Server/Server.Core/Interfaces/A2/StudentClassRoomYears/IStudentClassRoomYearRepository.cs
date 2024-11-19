using AMMS.Core.Repositories;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.StudentClassRoomYears.Reponses;
using Server.Core.Interfaces.A2.StudentClassRoomYears.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.StudentClassRoomYears;

public interface IStudentClassRoomYearRepository : IAsyncRepository<StudentClassRoomYear>
{

    Task<Result<StudentClassRoomYear>> SaveAsync(StudentClassRoomYear data);
    Task<Result<StudentClassRoomYear>> SaveDataAsync(StudentClassRoomYear data);


    Task<Result<StudentClassRoomYear>> GetById(string id);
    Task<List<ClassRoomYearReportResponse>> GetAlls(ClassRoomYearFilterRequest request);
    Task<List<StudentClassRoomYear>> Gets(bool actived = true);
    Task<Result<StudentClassRoomYear>> UpdateAsync(StudentClassRoomYear data);
    Task<Result<StudentClassRoomYear>> ActiveAsync(ActiveRequest data);
    Task<Result<StudentClassRoomYear>> InActiveAsync(InactiveRequest data);
}



