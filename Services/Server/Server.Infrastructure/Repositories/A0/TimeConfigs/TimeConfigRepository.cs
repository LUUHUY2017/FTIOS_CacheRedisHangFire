using Server.Core.Entities.A0;
using Server.Core.Interfaces.A0;
using Server.Infrastructure.Datas.MasterData;

namespace Server.Infrastructure.Repositories.A0.TimeConfigs;


public class TimeConfigRepository : RepositoryBaseMasterData<A0_TimeConfig>, ITimeConfigRepository
{
    public TimeConfigRepository(MasterDataDbContext dbContext) : base(dbContext)
    {
    }
}
