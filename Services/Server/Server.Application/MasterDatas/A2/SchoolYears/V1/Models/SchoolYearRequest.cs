namespace Server.Application.MasterDatas.A2.SchoolYears.V1.Models;

public class SchoolYearRequest
{
    public string? Id { get; set; }
    public bool? Actived { get; set; }
    public string? Name { get; set; }
    public string? Depcription { get; set; }

    /// <summary>
    /// Ngày bắt đầu năm học
    /// </summary>
    public DateTime? Start { get; set; }
    /// <summary>
    /// Ngày kết thúc năm học
    /// </summary>
    public DateTime? End { get; set; }
}
