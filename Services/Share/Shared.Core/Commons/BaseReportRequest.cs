namespace Shared.Core.Commons
{
    public abstract class BaseReportRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? OrganizationId { get; set; }

        public int Export { get; set; } = 0;
        public int? RowsPerPage { get; set; } = 25;
        public int? CurentPage { get; set; } = 1;

        public string StartDateTime { get; set; } = DateTime.Now.ToLocalTime().Date.ToString("dd/MM/yyyy HH:mm:ss");
        public string EndDateTime { get; set; } = DateTime.Now.ToLocalTime().Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString("dd/MM/yyyy HH:mm:ss");
    }
}
