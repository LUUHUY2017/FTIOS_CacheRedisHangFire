using AMMS.Core.Repositories;
using Server.Core.Entities.A2;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.SchoolYears;

public interface ISchoolYearRepository : IAsyncRepository<SchoolYear>
{

    Task<Result<SchoolYear>> SaveAsync(SchoolYear data);
    Task<Result<SchoolYear>> SaveDataAsync(SchoolYear data);
}



