using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.DashBoardReports.Models;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.DashBoardReports
{
    public interface IDashBoardReportRepository
    {
        Task<Result<ScheduleSendMail>> GetById(string id);
        Task<List<ScheduleSendMail>> GetAlls(DashBoardReportModel request);
        Task<Result<ScheduleSendMail>> UpdateAsync(ScheduleSendMail data);
        Task<Result<ScheduleSendMail>> ActiveAsync(ActiveRequest data);
        Task<Result<ScheduleSendMail>> InActiveAsync(InactiveRequest data);
    }
}
