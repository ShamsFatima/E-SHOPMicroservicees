using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class DispatchDomainEventsInterceptor(IMediator mediator):SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvents(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        public async Task DispatchDomainEvents(DbContext? context)
        {
            if (context == null) return;
            var domainEntities = context.ChangeTracker
                .Entries<Domain.Abstractions.IAggregate>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .ToList();
            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();
            domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());
            // Here you would typically publish the domain events to a message bus or event handler
            foreach (var domainEvent in domainEvents)
            {
                // Simulate publishing the event
                await mediator.Publish(domainEvent);
            }
        }
    }
}
