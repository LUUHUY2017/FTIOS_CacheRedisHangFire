using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.SendEmails;

public interface ISendEmailLogRepository
{
    Task<Result<List<SendEmailLogs>>> Get(string sendEMailId);
    Task<Result<List<SendEmailLogs>>> GetAlls(ScheduleSendEmailLogModel request);
    Task<Result<SendEmailLogs>> UpdateAsync(SendEmailLogs data);
    Task<Result<int>> DeleteAsync(string id);
}


