namespace Shared.Core.Commons
{
    public abstract class BaseReportRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? OrganizationId { get; set; }

        public string Actived { get; set; } = "1";
        public int? RowsPerPage { get; set; } = 25;
        public int? CurentPage { get; set; } = 1;

        //public string StartDateTime { get; set; } = DateTime.Now.ToLocalTime().Date.ToString("dd/MM/yyyy HH:mm:ss");
        //public string EndDateTime { get; set; } = DateTime.Now.ToLocalTime().Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString("dd/MM/yyyy HH:mm:ss");

        public List<FilterItems>? FilterItems { get; set; } = new List<FilterItems>();
        public SortItem? SortItem { get; set; } = new SortItem();
    }

    public class FilterItems
    {
        public string PropertyName { get; set; }
        public int Comparison { get; set; } // 0: Equal, 1: GreaterThan, 2: LessThan, etc.
        public string Value { get; set; }
    }

    public class SortItem
    {
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
    }

}
