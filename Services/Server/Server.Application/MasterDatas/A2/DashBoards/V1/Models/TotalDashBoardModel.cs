using Server.Application.MasterDatas.A2.DashBoards.V1.Models.Devices;
using Server.Application.MasterDatas.A2.DashBoards.V1.Models.Schools;
using Server.Application.MasterDatas.A2.DashBoards.V1.Models.SendEmail;
using Server.Application.MasterDatas.A2.DashBoards.V1.Models.StudentAttendances;
using Server.Application.MasterDatas.A2.DashBoards.V1.Models.StudentFaces;

namespace Server.Application.MasterDatas.A2.DashBoards.V1.Models;

public class TotalDashBoardModel
{
    public TotalSchoolModel TotalSchoolModel { get; set; }
    public TotalSendEmailModel TotalSendEmailModel { get; set; }
    public DBDeviceModel1 DBDeviceModel1 { get; set; }
    public DBStudentFaceModel DBStudentFaceModel { get; set; }
    public DBStudentAttendaceModel StudentAttendaceModel { get; set; }

}
