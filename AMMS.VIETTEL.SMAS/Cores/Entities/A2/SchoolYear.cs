using Shared.Core.Entities;

namespace AMMS.VIETTEL.SMAS.Cores.Entities.A2;


public class SchoolYear : EntityBase
{
    /// <summary>
    /// Tên năm học
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Ngày bắt đầu năm học
    /// </summary>
    public DateTime? Start { get; set; }
    /// <summary>
    /// Ngày kết thúc năm học
    /// </summary>
    public DateTime? End { get; set; }
    public string? Depcription { get; set; }

}
