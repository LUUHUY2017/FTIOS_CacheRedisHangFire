
namespace Shared.Core;


public class DeviceData
{
    public DeviceProperties Propertie { get; set; }
    public string PublicIP { get; set; }

    /// <summary>
    /// Thời gian mới nhất nhận dữ liệu từ thiết bị gửi về API
    /// </summary>
    public DateTime ReceivedTime { get; set; }

    /// <summary>
    /// Thời gian mới nhất của dữ liệu đếm
    /// </summary>
    public DateTime LastimeReceivedData { get; set; }

    public List<PeopleCountData> PeopleCountingDatas { get; set; }
}

public class DeviceProperties
{
    public string SerialNumber { get; set; }
    public int Version { get; set; }
    public int TransmitTime { get; set; }
    public string MacAddress { get; set; }
    public string IpAddress { get; set; }
    public string HostName { get; set; }
    public int HttpPort { get; set; }
    public int HttpsPort { get; set; }
    public int Timezone { get; set; }
    public string TimezoneName { get; set; }
    public int DST { get; set; }
    public int Platform { get; set; }
}
public class PeopleCountData
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Enters { get; set; } = 0;
    public int Exits { get; set; } = 0;
    //public long UnixStartTime { get; set; } = 0;
    public int Status { get; set; } = 0;
}
