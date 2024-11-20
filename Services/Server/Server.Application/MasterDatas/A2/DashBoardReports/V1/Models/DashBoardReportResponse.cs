using Server.Core.Entities.A2;

namespace Server.Application.MasterDatas.A2.DashBoardReports.V1.Models
{
    public class DashBoardReportResponse : ScheduleSendMail
    {
        /// <summary>
        /// Loại file xuất báo cáo
        /// </summary>
        public string? ScheduleExportTypeName { get; set; }

        /// <summary>
        /// Loại báo cáo
        /// </summary>
        public string? ScheduleReportTypeName { get; set; }

        /// <summary>
        /// Tuần suất gửi
        /// </summary>
        public string? ScheduleSequentialSendingName { get; set; }

        /// <summary>
        /// Ngày lấy dữ liệu
        /// </summary>
        public string? ScheduleDataCollectName { get; set; }
    }
}
