using AMMS.VIETTEL.SMAS.Cores.Entities.A0;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.AppConfigs;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;

namespace AMMS.VIETTEL.SMAS.Infratructures.Repositories.AppConfigs;

public class AppConfigRepository : RepositoryBase<A0_AttendanceConfig>, IAppConfigRepository
{
    public AppConfigRepository(ViettelDbContext dbContext) : base(dbContext)
    {
    }
}
