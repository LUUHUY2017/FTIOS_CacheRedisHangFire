using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.SendEmails;

public interface ISendEmailLogRepository
{
    Task<Result<List<A2_SendEmailLog>>> Get(string sendEMailId);
    Task<Result<List<A2_SendEmailLog>>> GetAlls(ScheduleSendEmailLogModel request);
    Task<Result<A2_SendEmailLog>> UpdateAsync(A2_SendEmailLog data);
    Task<Result<int>> DeleteAsync(string id);
}


