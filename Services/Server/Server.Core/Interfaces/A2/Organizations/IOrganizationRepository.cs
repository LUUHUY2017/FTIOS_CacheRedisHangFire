using AMMS.Core.Repositories;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.Organizations;

public interface IOrganizationRepository : IAsyncRepository<Organization>
{
    Task<Result<List<Organization>>> GetAlls(OrganizationFilterRequest request);
    Task<Result<Organization>> GetById(string id);
    Task<Result<Organization>> UpdateAsync(Organization data);
    Task<Result<Organization>> ActiveAsync(ActiveRequest data);
    Task<Result<Organization>> InActiveAsync(InactiveRequest data);
}
