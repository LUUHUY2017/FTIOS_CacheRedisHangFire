using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.VIETTEL.SMAS.Cores.Entities.A2;

public class SendEmailLogs : EntityBase
{

    public string? SendEmailId { get; set; }
    public DateTime? TimeSent { get; set; }
    public string? MessageLog { get; set; }
    public bool? Sent { get; set; }
}
