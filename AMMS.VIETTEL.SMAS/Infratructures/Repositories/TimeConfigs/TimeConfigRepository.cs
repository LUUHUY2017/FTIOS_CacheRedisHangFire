using AMMS.VIETTEL.SMAS.Cores.Entities.A0;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.TimeConfigs;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;

namespace AMMS.VIETTEL.SMAS.Infratructures.Repositories.AppConfigs;


public class TimeConfigRepository : RepositoryBase<TimeConfig>, ITimeConfigRepository
{
    public TimeConfigRepository(ViettelDbContext dbContext) : base(dbContext)
    {
    }
}
