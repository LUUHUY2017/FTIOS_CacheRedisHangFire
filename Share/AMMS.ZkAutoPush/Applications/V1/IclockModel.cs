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
        public bool IsSuccessed { get; set; } = false;

        public string UserID { get; set; } = "";
        public bool IsSystemCommand { get; set; } = false;

        public IclockDataTable DataTable { get; set; }

        public string DataId { get; set; }
        public string? ParentId { get; set; }
        public string Action { get; set; } = "";
        public DateTime RevicedTime { get; set; } = DateTime.Now;
        public DateTime CommitTime { get; set; } = DateTime.Now;
        public int? returnCode { get; set; }
    }


    public enum IclockDataTable
    {
        None, All, A2NguoiIclockSyn, A2VanTayIclockSyn, A2NguoiIclockUserPicSyn
    }
}
