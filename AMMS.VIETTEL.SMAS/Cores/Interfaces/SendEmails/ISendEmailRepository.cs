using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleSendEmails;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Cores.Interfaces.SendEmails;

public interface ISendEmailRepository
{
    Task<Result<List<Entities.A2.SendEmails>>> GetAlls(ScheduleSendEmailLogModel request);
    Task<Result<Entities.A2.SendEmails>> UpdateAsync(Entities.A2.SendEmails data);
    Task<Result<int>> DeleteAsync(string id);
    Task<Result<EmailConfiguration>> GetEmailConfiguration(string orgId);
}


