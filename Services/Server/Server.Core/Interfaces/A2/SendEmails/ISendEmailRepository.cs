using Server.Core.Entities.A0;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.SendEmails;

public interface ISendEmailRepository
{
    Task<Result<List<A2_SendEmail>>> GetAlls(ScheduleSendEmailLogModel request);
    Task<Result<A2_SendEmail>> UpdateAsync(A2_SendEmail data);
    Task<Result<int>> DeleteAsync(string id);
    Task<Result<A0_EmailConfiguration>> GetEmailConfiguration(string orgId);
}


