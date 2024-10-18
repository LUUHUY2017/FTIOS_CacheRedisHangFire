using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Core.Entities;

namespace AMMS.Infrastructure.Data;

public abstract class BaseDbContext : DbContext
{
    private readonly IMediator _mediator;

    public BaseDbContext() : base()
    {

    }
    public BaseDbContext(DbContextOptions options, IMediator mediator) : base(options)
    {
        //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
        _mediator = mediator;
    }


    public async Task<int> SaveChangesAsync()
    {
        await _dispatchDomainEvents();
        var response = await base.SaveChangesAsync();
        return response;
    }
    private async Task _dispatchDomainEvents()
    {
        var domainEventEntities = ChangeTracker.Entries<EntityBase>()
            .Select(po => po.Entity)
            .Where(po => po.DomainEvents.Any())
            .ToArray();

        foreach (var entity in domainEventEntities)
        {
            var events = entity.DomainEvents.ToArray();
            foreach (var entityDomainEvent in events)
                await _mediator.Publish(entityDomainEvent);
            entity.DomainEvents.Clear();
        }
    }
}
