namespace AMMS.VIETTEL.SMAS.Applications.Services.VTSmart.Responses
{
    public class SchoolYearResponse
    {
        public string? Code { get; set; }
        public string? SchoolYearId { get; set; }
        public string? TenantId { get; set; }
        public bool CurrentYear { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? FirstSemesterStartDate { get; set; }
        public DateTime? FirstSemesterEndDate { get; set; }
        public DateTime? SecondSemesterStartDate { get; set; }
        public DateTime? SecondSemesterEndDate { get; set; }
        public string? PrincipalName { get; set; }
        public string? PrincipalId { get; set; }
        public bool? IsActive { get; set; }
        public string? Description { get; set; }
        public int? Sort { get; set; }
    }
}
