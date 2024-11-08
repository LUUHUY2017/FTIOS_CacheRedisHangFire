using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.A2;

[Table("PersonType")]
public class A2_PersonType : EntityBase
{
    public string? Name { get; set; }
    public string? Note { get; set; }
}
