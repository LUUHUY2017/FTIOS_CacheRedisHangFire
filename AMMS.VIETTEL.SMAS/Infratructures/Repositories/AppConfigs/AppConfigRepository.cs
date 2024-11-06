using AMMS.VIETTEL.SMAS.Cores.Entities;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.AppConfigs;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;

namespace AMMS.VIETTEL.SMAS.Infratructures.Repositories.AppConfigs;

public class AppConfigRepository : RepositoryBase<app_config>, IAppConfigRepository
{
    public AppConfigRepository(ViettelDbContext dbContext) : base(dbContext)
    {
    }
}
