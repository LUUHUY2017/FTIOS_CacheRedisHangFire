namespace AMMS.Notification.Datas.Entities;

public partial class SendEmailLog
{
    public Guid Id { get; set; }

    public Guid SendEmailId { get; set; }

    public DateTime? TimeSent { get; set; }
    public string? MessageLog { get; set; }
    public bool? Sent { get; set; }

}

