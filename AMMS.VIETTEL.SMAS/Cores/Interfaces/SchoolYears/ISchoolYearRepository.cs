using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using Shared.Core.Commons;
using Shared.Core.Repositories;

namespace AMMS.VIETTEL.SMAS.Cores.Interfaces.SchoolYears;

public interface ISchoolYearRepository : IAsyncRepository<SchoolYear>
{

    Task<Result<SchoolYear>> SaveAsync(SchoolYear data);
    Task<Result<SchoolYear>> SaveDataAsync(SchoolYear data);
}



