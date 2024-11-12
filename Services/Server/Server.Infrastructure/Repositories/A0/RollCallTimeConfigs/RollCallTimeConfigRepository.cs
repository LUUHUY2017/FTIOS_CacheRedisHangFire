using Server.Core.Entities.A0;
using Server.Core.Interfaces.A0;
using Server.Infrastructure.Datas.MasterData;

namespace Server.Infrastructure.Repositories.A0.RollCallTimeConfigs;

public class RollCallTimeConfigRepository : RepositoryBaseMasterData<RollCallTimeConfig>, IRollCallTimeConfigRepository
{
    public RollCallTimeConfigRepository(MasterDataDbContext dbContext) : base(dbContext)
    {
    }
}
