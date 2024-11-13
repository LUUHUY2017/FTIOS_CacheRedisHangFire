using Shared.Core.Entities;

namespace Server.Core.Entities.A2;

public class SchoolYear : EntityBase
{
    /// <summary>
    /// Tên năm học
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Ngày bắt đầu năm học
    /// </summary>
    public DateTime Start { get; set; }
    /// <summary>
    /// Ngày kết thúc năm học
    /// </summary>
    public DateTime End { get; set; }
    public string Depcription { get; set; }

}

///// <summary>
///// Lịch sử điểm danh
///// </summary>
//public class StudentAttendances : EntityBase
//{
//    public string SchoolId { get; private set; }
//    public string StudentClassRoomYearId { get; private set; }
//    public string StudentId { get; private set; }

//}