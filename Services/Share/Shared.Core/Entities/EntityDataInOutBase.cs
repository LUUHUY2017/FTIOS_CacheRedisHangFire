using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Core.Entities
{
    public abstract class EntityDataInOutBase//: IEntityBase
    {
        //Protected set is made to use in the derived classes
        public decimal Id { get; set; }
        //Below Properties are Audit properties
        //public string? CreatedBy { get; set; }
        //public DateTime? CreatedDate { get; set; } = DateTime.Now;
        //public string? LastModifiedBy { get; set; }
        //public DateTime? LastModifiedDate { get; set; } = DateTime.Now;
        //public bool? Actived { get; set; }
        //public bool? Deleted { get; set; }
        //[MaxLength(500)]
        //public string? ReasonDelete { get; set; }

        [NotMapped]
        public List<IDomainEvent> DomainEvents { get; } = new List<IDomainEvent>();

        public void QueueDomainEvent(IDomainEvent @event)
        {
            DomainEvents.Add(@event);
        }

    }
}
