using Shared.Core.Entities;

namespace Server.Core.Entities.A2;

public class A2_BusinessUnit : EntityBase
{
    public Guid? ParentId { get; set; }
    public Guid? OrganizationId { get; set; }
}