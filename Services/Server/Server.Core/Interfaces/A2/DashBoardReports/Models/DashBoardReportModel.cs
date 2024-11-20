namespace Server.Core.Interfaces.A2.DashBoardReports.Models
{
    public class DashBoardReportModel
    {
        public string? OrganizationId { get; set; }
        public string? ColumnTable { get; set; }
        public string? Actived { get; set; }
        public string? Key { get; set; }
        public string? Note { get; set; } = "";
        public string? Export { get; set; } = "0";
    }

    public class DashBoardReportLogModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Sent { get; set; } = "";

    }
}
