using Server.Core.Entities.A0;
using Server.Core.Interfaces.A0;
using Server.Infrastructure.Datas.MasterData;

namespace Server.Infrastructure.Repositories.A0.AttendanceTimeConfigs;

public class AttendanceTimeConfigRepository : RepositoryBaseMasterData<AttendanceTimeConfig>, IAttendanceTimeConfigRepository
{
    public AttendanceTimeConfigRepository(MasterDataDbContext dbContext) : base(dbContext)
    {
    }
}
