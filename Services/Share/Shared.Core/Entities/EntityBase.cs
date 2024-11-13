using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Core.Entities;
//This will serve as common fields for domain
//This means, every entity will have below props by default in ordering Microservice
//public interface IEntityBase
//{
//    public string? CreatedBy { get; set; }

//}
public abstract class EntityBase//: IEntityBase
{
    //Protected set is made to use in the derived classes
    private string _Id { get; set; }
    [MaxLength(50)]
    public string Id { get { if (string.IsNullOrEmpty(_Id)) _Id = Guid.NewGuid().ToString(); return _Id; } set { _Id = value; } }
    //Below Properties are Audit properties
    [MaxLength(50)]
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    [MaxLength(50)]
    public string? LastModifiedBy { get; set; }
    public DateTime LastModifiedDate { get; set; } = DateTime.Now;
    public bool? Actived { get; set; } = true;
    [MaxLength(1000)]
    public string? Reason { get; set; }
    public string? Logs { get; set; }

    [MaxLength(50)]
    public string? OrganizationId { get; set; }
    public string? ReferenceId { get; set; }

    [NotMapped]
    public List<IDomainEvent> DomainEvents { get; } = new List<IDomainEvent>();

    public void QueueDomainEvent(IDomainEvent @event)
    {
        DomainEvents.Add(@event);
    }

}