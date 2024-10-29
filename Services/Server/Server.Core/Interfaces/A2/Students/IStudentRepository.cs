using AMMS.Core.Repositories;
using Server.Core.Entities.A2;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.Students;

public interface IStudentRepository : IAsyncRepository<A2_Student>
{

    Task<Result<A2_Student>> SaveAsync(A2_Student data);

}



