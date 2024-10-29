namespace AMMS.ZkAutoPush.Data
{
    public class ZK_DEVICE_RP
    {
        public DateTime ReceivedTime { get; set; } = DateTime.Now;
        public string Content { get; set; }
        public string ReceivedIp { get; set; }
        public string SN { get; set; }

    }
}
