using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos.Exceptions;
using Ordering.Application.Orders.Commands.UpdateOrder;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands.DeleteOrder;

public record DeleteOrderHandler(IApplicationDbContext applicationDbContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(request.Id);
        var order = await applicationDbContext.Orders.FindAsync(new object?[] { orderId }, cancellationToken);
        if (order is null)
        {
            throw new OrderNotFoundException(request.Id);
        }
      
        applicationDbContext.Orders.Update(order);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return new DeleteOrderResult(true);
    }
}
