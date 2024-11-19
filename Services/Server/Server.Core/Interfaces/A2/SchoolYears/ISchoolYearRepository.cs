using AMMS.Core.Repositories;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.SchoolYears.Reponses;
using Server.Core.Interfaces.A2.SchoolYears.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.SchoolYears;

public interface ISchoolYearRepository : IAsyncRepository<SchoolYear>
{

    Task<Result<SchoolYear>> SaveAsync(SchoolYear data);
    Task<Result<SchoolYear>> SaveDataAsync(SchoolYear data);

    Task<Result<SchoolYear>> GetById(string id);
    Task<List<SchoolYearReportResponse>> GetAlls(SchoolYearFilterRequest request);
    Task<List<SchoolYear>> Gets(bool actived = true);
    Task<Result<SchoolYear>> UpdateAsync(SchoolYear data);
    Task<Result<SchoolYear>> ActiveAsync(ActiveRequest data);
    Task<Result<SchoolYear>> InActiveAsync(InactiveRequest data);
}



