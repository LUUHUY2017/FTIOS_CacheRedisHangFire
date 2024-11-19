using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace AMMS.VIETTEL.SMAS.Cores.Entities.A2;


/// <summary>
/// Học sinh theo lớp, năm học
/// </summary>
public class StudentClassRoomYear : EntityBase
{
    /// <summary>
    /// ID trường học
    /// </summary>
    [MaxLength(50)]
    public string? SchoolId { get; set; }

    /// <summary>
    /// Id lớp học
    /// </summary>
    [MaxLength(50)]
    public string? ClassRoomId { get; set; }

    /// <summary>
    /// Id Năm học
    /// </summary>
    /// 
    [MaxLength(50)]
    public string? SchoolYearId { get; set; }


    /// <summary>
    /// Tên lớp - năm học
    /// </summary>
    public string? Name { get; set; }
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