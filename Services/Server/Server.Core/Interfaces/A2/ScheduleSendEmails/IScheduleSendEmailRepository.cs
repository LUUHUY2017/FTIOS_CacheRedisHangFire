using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.ScheduleSendEmails;

public interface IScheduleSendMailRepository
{
    Task<Result<ScheduleSendMail>> GetById(string id);
    Task<List<ScheduleSendMail>> GetAlls(ScheduleSendEmailFilterRequest request);
    Task<Result<ScheduleSendMail>> UpdateAsync(ScheduleSendMail data);
    Task<Result<ScheduleSendMail>> ActiveAsync(ActiveRequest data);
    Task<Result<ScheduleSendMail>> InActiveAsync(InactiveRequest data);



}



