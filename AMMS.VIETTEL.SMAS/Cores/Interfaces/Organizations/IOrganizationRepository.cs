using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Organizations.Requests;
using Shared.Core.Commons;
using Shared.Core.Repositories;

namespace AMMS.VIETTEL.SMAS.Cores.Interfaces.Organizations;

public interface IOrganizationRepository : IAsyncRepository<A2_Organization>
{
    Task<Result<List<A2_Organization>>> GetAlls(OrganizationFilterRequest request);
    Task<Result<A2_Organization>> GetById(string id);
    Task<Result<A2_Organization>> UpdateAsync(A2_Organization data);
    Task<Result<A2_Organization>> ActiveAsync(ActiveRequest data);
    Task<Result<A2_Organization>> InActiveAsync(InactiveRequest data);
}
