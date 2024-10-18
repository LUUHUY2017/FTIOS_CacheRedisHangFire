namespace Server.API.APIs.MileSights.V1.Model
{
    public sealed class MilesightLrpCameraMessage
    {
        public string device { get; set; }
        public string plate { get; set; }
        public string type { get; set; }
        public string speed { get; set; }
        public string direction { get; set; }
        public string detection_region { get; set; }
        public string region { get; set; }
        public int resolution_width { get; set; }
        public int resolution_height { get; set; }
        public int coordinate_x1 { get; set; }
        public int coordinate_y1 { get; set; }
        public int coordinate_x2 { get; set; }
        public int coordinate_y2 { get; set; }
        public float confidence { get; set; }


        public string plate_image { get; set; }
        public string full_image { get; set; }
    }
}
