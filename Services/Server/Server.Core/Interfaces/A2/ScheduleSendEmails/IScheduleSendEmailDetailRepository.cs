using Server.Core.Entities.A2;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.ScheduleSendEmails;

public interface IScheduleSendEmailDetailRepository
{
    Task<Result<List<ScheduleSendMailDetail>>> Get(string sheduleId);
    Task<Result<ScheduleSendMailDetail>> UpdateAsync(ScheduleSendMailDetail product);
    Task<Result<List<ScheduleSendMailDetail>>> UpdateAsync(List<ScheduleSendMailDetail> datas);
    Task<Result<int>> DeleteAsync(DeleteRequest request);

}
