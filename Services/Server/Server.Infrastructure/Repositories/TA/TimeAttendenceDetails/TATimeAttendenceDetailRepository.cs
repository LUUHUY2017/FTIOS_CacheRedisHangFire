using Server.Core.Interfaces.TA.TimeAttendenceDetails;
using Server.Infrastructure.Datas.MasterData;

namespace Server.Infrastructure.Repositories.GIO.TimeAttendenceDetails;

public class TATimeAttendenceDetailRepository : ITATimeAttendenceDetailRepository
{
    private readonly IMasterDataDbContext _db;
    public TATimeAttendenceDetailRepository(IMasterDataDbContext biDbContext)
    {
        _db = biDbContext;
    }


}