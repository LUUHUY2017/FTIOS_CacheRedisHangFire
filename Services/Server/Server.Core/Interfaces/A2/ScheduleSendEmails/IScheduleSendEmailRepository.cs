using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.ScheduleSendEmails;

public interface IScheduleSendMailRepository
{
    Task<Result<A2_ScheduleSendMail>> GetById(string id);
    Task<List<A2_ScheduleSendMail>> GetAlls(ScheduleSendEmailModel request);
    Task<Result<A2_ScheduleSendMail>> UpdateAsync(A2_ScheduleSendMail data);
    Task<Result<A2_ScheduleSendMail>> ActiveAsync(ActiveRequest data);
    Task<Result<A2_ScheduleSendMail>> InActiveAsync(InactiveRequest data);



}



