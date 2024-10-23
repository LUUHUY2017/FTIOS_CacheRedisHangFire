using Shared.Core.Entities;

namespace Server.Core.Entities.A2;

public class A2_BusinessUnit : EntityBase
{
    public string? ParentId { get; set; }
    public string? OrganizationId { get; set; }
}