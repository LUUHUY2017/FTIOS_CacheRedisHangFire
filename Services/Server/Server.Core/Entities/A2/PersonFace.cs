using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.A0;

public class PersonFace : EntityBase
{
    [MaxLength(50)]
    public string? PersonId { get; set; }

    public string? FaceData { get; set; }
    [MaxLength(50)]
    public string? FaceType { get; set; }
    public int? FaceIndex { get; set; }
}