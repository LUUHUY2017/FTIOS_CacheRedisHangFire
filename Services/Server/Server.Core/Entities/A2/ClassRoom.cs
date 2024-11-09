using Shared.Core.Entities;

namespace Server.Core.Entities.A2;

public class ClassRoom : EntityBase
{ 
    ///// <summary>
    ///// ID trường học
    ///// </summary>
    //public string SchoolId { get;set; }

    public Organization? Organization { get; set; }


    /// <summary>
    /// Tên lớp học
    /// </summary>
    public string Name { get; set; }
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