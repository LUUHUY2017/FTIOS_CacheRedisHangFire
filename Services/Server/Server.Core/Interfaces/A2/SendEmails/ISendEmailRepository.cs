using Server.Core.Entities.A0;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.SendEmails;

public interface ISendEmailRepository
{
    Task<Result<List<Entities.A2.SendEmails>>> GetAlls(ScheduleSendEmailLogModel request);
    Task<Result<Entities.A2.SendEmails>> UpdateAsync(Entities.A2.SendEmails data);
    Task<Result<int>> DeleteAsync(string id);
    Task<Result<EmailConfiguration>> GetEmailConfiguration(string orgId);
}


