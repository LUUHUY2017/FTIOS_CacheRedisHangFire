using Server.Core.Entities.A2;

<<<<<<< HEAD:Services/Server/Server.Application/MasterDatas/A2/ScheduleJobs/V1/Models/ScheduleJobResponse.cs
namespace Server.Application.MasterDatas.A2.ScheduleJobs.V1.Models;
=======
namespace Server.API.APIs.Data.ScheduleJobs.V1.Responses;
>>>>>>> 9969c0e0f72576e1dab2f3266bdef031653b21d4:Services/Server/Server.API/APIs/Data/ScheduleJobs/V1/Responses/ScheduleJobResponse.cs
public class ScheduleJobResponse : ScheduleJob
{
    /// <summary>
    /// Loại đồng bộ
    /// </summary>
    public string? ScheduleJobTypeName { get; set; }

    /// <summary>
    /// Tuần suất gửi
    /// </summary>
    public string? ScheduleSequentialSendingName { get; set; }

}
