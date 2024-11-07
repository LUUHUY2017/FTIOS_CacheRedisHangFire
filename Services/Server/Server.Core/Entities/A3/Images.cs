using Shared.Core.Entities;

namespace Server.Core.Entities.A3
{
    public class Images : EntityBase
    {
        public string? ImageName { get; set; }
        public string? ImageFolder { get; set; }

        /// <summary>
        /// Vị trí ảnh
        /// </summary>
        public int? ImageIndex { get; set; }
        /// <summary>
        /// Ảnh biển số nhận dạng
        /// </summary>
        public bool? IsPlateNumberImage { get; set; }
        /// <summary>
        /// Lần 1,2,3
        /// </summary>
        public int? TimeWeight { get; set; }
        public string? ImageBase64 { get; set; }


    }
}
