using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.A2;

public class Lane : EntityBase
{
    [MaxLength(250)]
    public string? LaneCode { get; set; }
    public string? LaneName { get; set; }
    public string? Description { get; set; }
    public string? GateId { get; set; }

}


