using Shared.Core.Commons;

namespace Server.Application.MasterDatas.A2.Students.V1.Model;
public class StudentSearchRequest : BaseReportRequest
{
    public string? ClassId { get; set; }
    public string? SchoolyearId { get; set; }
}


