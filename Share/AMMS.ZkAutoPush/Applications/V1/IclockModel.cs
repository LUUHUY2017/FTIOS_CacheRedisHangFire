namespace AMMS.ZkAutoPush.Applications.V1
{
 
    public class IclockCommand
    {
        public IclockCommand()
        {

        }
        public double Id { get; set; }
        public string SerialNumber { get; set; } = "";
        public string Command { get; set; } = "";
        public bool IsRequest { get; set; } = false;

        public string UserID { get; set; } = "";
        public bool IsSystemCommand { get; set; } = false;

        public IclockDataTable DataTable { get; set; }

        public string DataId { get; set; }
        public string Action { get; set; } = "";

    }

    public class IclockDevice
    {
        public IclockDevice()
        {

        }
        public string Id { get; set; }
        public string SerialNumber { get; set; } = "";
        public string Command { get; set; } = "";
        public string IpAddress { get; set; } = "";
        public string Password { get; set; } = "";
        public string UserName { get; set; } = "";
        public string DataPort { get; set; } = "";
        public string HttpPort { get; set; } = "";
        public bool Active { get; set; } = true;
        public DateTime? ThoiGianCapNhatKetNoi { get; set; }

    }
    public enum IclockDataTable
    {
        None, All, A2NguoiIclockSyn, A2VanTayIclockSyn, A2NguoiIclockUserPicSyn
    }
}
