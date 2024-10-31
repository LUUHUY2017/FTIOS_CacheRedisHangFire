using AMMS.Core.Repositories;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.Organizations;

public interface IOrganizationRepository : IAsyncRepository<A2_Organization>
{
    Task<Result<List<A2_Organization>>> GetAlls(OrganizationFilterRequest request);
    Task<Result<A2_Organization>> GetById(string id);
    Task<Result<A2_Organization>> UpdateAsync(A2_Organization data);
    Task<Result<A2_Organization>> ActiveAsync(ActiveRequest data);
    Task<Result<A2_Organization>> InActiveAsync(InactiveRequest data);
}
