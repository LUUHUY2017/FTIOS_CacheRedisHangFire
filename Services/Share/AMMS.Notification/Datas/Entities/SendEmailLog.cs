using Shared.Core.Entities;

namespace AMMS.Notification.Datas.Entities;

public class SendEmailLog : EntityBase
{

    public string? SendEmailId { get; set; }
    public DateTime? TimeSent { get; set; }
    public string? MessageLog { get; set; }
    public bool? Sent { get; set; }
}

