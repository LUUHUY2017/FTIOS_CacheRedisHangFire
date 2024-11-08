using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.A2;

public class BusinessUnit : EntityBase
{
    public string? ParentId { get; set; }
    public string? OrganizationId { get; set; }
}