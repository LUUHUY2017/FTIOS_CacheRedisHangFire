using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleSendEmails;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Cores.Interfaces;

public interface IScheduleSendMailRepository
{
    Task<Result<ScheduleSendMail>> GetById(string id);
    Task<List<ScheduleSendMail>> GetAlls(ScheduleSendEmailFilterRequest request);
    Task<Result<ScheduleSendMail>> UpdateAsync(ScheduleSendMail data);
    Task<Result<ScheduleSendMail>> ActiveAsync(ActiveRequest data);
    Task<Result<ScheduleSendMail>> InActiveAsync(InactiveRequest data);



}



