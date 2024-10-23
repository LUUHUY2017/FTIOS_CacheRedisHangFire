using Server.Core.Entities.A2;
using Shared.Core.Commons;
using Shared.Core.Identity.Object;

namespace Server.Core.Interfaces.A2.Students;

public interface IStudentRepository
{
    Task<Result<A2_Student>> GetById(string id);
    Task<List<A2_Student>> GetAll();
    Task<Result<A2_Student>> UpdateAsync(A2_Student data);
    Task<Result<A2_Student>> ActiveAsync(ActiveRequest data);
    Task<Result<A2_Student>> InActiveAsync(InactiveRequest data);



}



