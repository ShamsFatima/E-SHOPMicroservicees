using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.EventHandlers.EventHandlers
{
    public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger) :
        INotificationHandler<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("OrderCreatedEvent handled for OrderId: {OrderId}", notification.Order.Id);
            return Task.CompletedTask;
        }
    }
}
