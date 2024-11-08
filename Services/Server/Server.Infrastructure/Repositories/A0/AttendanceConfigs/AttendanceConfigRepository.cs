using Server.Core.Entities.A0;
using Server.Core.Interfaces.A0;
using Server.Infrastructure.Datas.MasterData;

namespace Server.Infrastructure.Repositories.A0.AttendanceConfigs;

public class AttendanceConfigRepository : RepositoryBaseMasterData<AttendanceConfig>, IAttendanceConfigRepository
{
    public AttendanceConfigRepository(MasterDataDbContext dbContext) : base(dbContext)
    {
    }
}
