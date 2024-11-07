using AMMS.Core.Repositories;
using Server.Core.Entities.A2;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.Students;

public interface IStudentRepository : IAsyncRepository<Student>
{

    Task<Result<Student>> SaveAsync(Student data);
    Task<Result<Student>> SaveDataAsync(Student data);
}



