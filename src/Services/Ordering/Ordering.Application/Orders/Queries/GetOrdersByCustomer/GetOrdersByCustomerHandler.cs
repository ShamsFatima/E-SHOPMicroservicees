using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    public class GetOrdersByCustomerHandler(IApplicationDbContext applicationDbContext) :
                IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
    {
        public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
        {
            var orders = await applicationDbContext.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .Where(o => o.CustomerId == CustomerId.Of(request.CustomerId))
                .OrderBy(o => o.OrderName.Value)
                .ToListAsync(cancellationToken);
            //var orderDtos= ProjectToOrdersDto(orders);
            return new GetOrdersByCustomerResult(orders.ToOrderDtoList());
        }
    }
}
