using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Server.Core.Entities.A2;

public class A2_Notification : EntityBase
{
    public string? UserId { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public string? Data { get; set; }
    public bool? Readed { get; set; }
    public DateTime? ReadedTime { get; set; }

}
