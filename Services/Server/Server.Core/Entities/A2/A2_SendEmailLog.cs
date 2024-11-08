using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.A2;

[Table("SendEmailLog")]

public class A2_SendEmailLog : EntityBase
{

    public string? SendEmailId { get; set; }
    public DateTime? TimeSent { get; set; }
    public string? MessageLog { get; set; }
    public bool? Sent { get; set; }
}
