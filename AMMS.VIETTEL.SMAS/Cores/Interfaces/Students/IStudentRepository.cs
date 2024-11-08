using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using Shared.Core.Commons;
using Shared.Core.Repositories;

namespace AMMS.VIETTEL.SMAS.Cores.Interfaces.Students;

public interface IStudentRepository : IAsyncRepository<Student>
{

    Task<Result<Student>> SaveAsync(Student data);
    Task<Result<Student>> SaveDataAsync(Student data);
}



