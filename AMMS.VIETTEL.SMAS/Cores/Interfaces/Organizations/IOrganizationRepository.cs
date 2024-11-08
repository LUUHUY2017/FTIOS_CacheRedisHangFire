using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Organizations.Requests;
using Shared.Core.Commons;
using Shared.Core.Repositories;

namespace AMMS.VIETTEL.SMAS.Cores.Interfaces.Organizations;

public interface IOrganizationRepository : IAsyncRepository<Organization>
{
    Task<Result<List<Organization>>> GetAlls(OrganizationFilterRequest request);
    Task<Result<Organization>> GetById(string id);
    Task<Result<Organization>> UpdateAsync(Organization data);
    Task<Result<Organization>> ActiveAsync(ActiveRequest data);
    Task<Result<Organization>> InActiveAsync(InactiveRequest data);
}
