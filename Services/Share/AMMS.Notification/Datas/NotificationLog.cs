using MassTransit.Util;
using MongoDB.Driver.Core.Operations;
using Shared.Core.Entities;

namespace AMMS.Notification.Datas;

public class Notification : EntityBase
{
    public string? Application { get; set; }
    public string? UserId { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    public string? ImageUrl { get; set; }
    public string? Data { get; set; }
    public bool? Readed { get; set; }
    public DateTime? ReadedTime { get; set; }
    public string? Type { get; set; }
}
