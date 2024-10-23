using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Organizations;
using Server.Infrastructure.Datas.MasterData;

namespace Server.Infrastructure.Repositories.A2.Organizations;

public class OrganizationRepository : RepositoryBaseMasterData<A2_Organization>, IOrganizationRepository
{
    public OrganizationRepository(MasterDataDbContext dbContext) : base(dbContext)
    {
    }
}
