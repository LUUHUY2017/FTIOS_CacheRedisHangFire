namespace AMMS.ZkAutoPush.Data;

public class ZK_TA_DATA
{
    public DateTime ReceivedTime { get; set; } = DateTime.Now;
    public string Content { get; set; }
    public string ReceivedIp { get; set; }
    public string SN { get; set; }
    public string Table { get; set; }
    public string Options { get; set; }
}