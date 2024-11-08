using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Applications.Services.Students.V1.Model;
public class StudentSearchRequest : BaseReportRequest
{
    public string? ClassId { get; set; }
    public string? SchoolyearId { get; set; }
}


