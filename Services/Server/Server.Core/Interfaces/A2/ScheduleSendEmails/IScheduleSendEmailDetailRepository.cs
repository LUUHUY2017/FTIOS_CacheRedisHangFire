using Server.Core.Entities.A2;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.ScheduleSendEmails;

public interface IScheduleSendEmailDetailRepository
{
    Task<Result<List<A2_ScheduleSendMailDetail>>> Get(string sheduleId);
    Task<Result<A2_ScheduleSendMailDetail>> UpdateAsync(A2_ScheduleSendMailDetail product);
    Task<Result<List<A2_ScheduleSendMailDetail>>> UpdateAsync(List<A2_ScheduleSendMailDetail> datas);
    Task<Result<int>> DeleteAsync(DeleteRequest request);

}
