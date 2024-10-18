using Server.Core.Entities.A3;

namespace Server.Core.Interfaces.GIO.VehicleInOuts.Responses
{
    public class ImageViewReportResponse
    {
        public A3_Image bien_so_xe_truoc_lan1 { get; set; } = null;
        public A3_Image bien_so_xe_sau_lan1 { get; set; } = null;
        public A3_Image anh_chup_truoc_lan1 { get; set; } = null;
        public A3_Image anh_chup_sau_lan1 { get; set; } = null;
        public A3_Image anh_toancanh_lan1 { get; set; } = null;

        public A3_Image bien_so_xe_truoc_lan2 { get; set; } = null;
        public A3_Image bien_so_xe_sau_lan2 { get; set; } = null;
        public A3_Image anh_chup_truoc_lan2 { get; set; } = null;
        public A3_Image anh_chup_sau_lan2 { get; set; } = null;
        public A3_Image anh_toancanh_lan2 { get; set; } = null;
    }
}
