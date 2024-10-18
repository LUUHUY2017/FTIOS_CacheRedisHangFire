namespace Server.Infrastructure.DataInOutCaching.Models
{
    public class DataInOutEventFilter : CommonFilter
    {
        public int? OrganizationId { get; set; } = -1;
        public int? SiteId { get; set; } = -1;
        public int? LocationId { get; set; } = -1;
        public string? StartTime { get; set; } = "";
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;

        public DateTime? ConvertStartTime()
        {
            DateTime parsedDateTime;
            DateTime.TryParse(StartTime, null, System.Globalization.DateTimeStyles.RoundtripKind, out parsedDateTime);
            return parsedDateTime;
        }
    }
}
