namespace AMMS.VIETTEL.SMAS.Applications.Services.VTSmart.Responses;

public class StudentResponse
{
    public string? Id { get; set; }
    public string? StudentCode { get; set; }
    public string? FullName { get; set; }
    public string? DateOfBirth { get; set; }
    public string? GenderCode { get; set; }
    public string? ImageSrc { get; set; }
    public string? ClassId { get; set; }
    public string? ClassName { get; set; }
    public bool? IsExemptedFull { get; set; }
    public string? StatusCode { get; set; }
    public string? Status { get; set; }
    public string? FullNameOther { get; set; }
    public string? EthnicCode { get; set; }
    public string? PolicyTargetCode { get; set; }
    public string? PriorityEncourageCode { get; set; }
    public string? SyncCode { get; set; }
    public string? SyncCodeClass { get; set; }
    public string? IdentifyNumber { get; set; }
    public string? StudentClassId { get; set; }
    public int? SortOrder { get; set; }
    public string? Name { get; set; }
    public int? SortOrderByClass { get; set; }
    public string? GradeCode { get; set; }
}

public class StudentResponse1
{
    public List<StudentInfo> responses { get; set; }
    public bool isSuccess { get; set; }
    public string message { get; set; }
}

public class StudentInfo
{
    public string studentCode { get; set; }
    public string studentName { get; set; }
    public string className { get; set; }
    public string classId { get; set; }
    public string birthDay { get; set; }
}