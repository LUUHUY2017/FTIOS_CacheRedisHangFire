using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleSendEmails;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Cores.Interfaces.SendEmails;

public interface ISendEmailLogRepository
{
    Task<Result<List<SendEmailLogs>>> Get(string sendEMailId);
    Task<Result<List<SendEmailLogs>>> GetAlls(ScheduleSendEmailLogModel request);
    Task<Result<SendEmailLogs>> UpdateAsync(SendEmailLogs data);
    Task<Result<int>> DeleteAsync(string id);
}


