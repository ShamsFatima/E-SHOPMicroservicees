using BuildingBlocks.CQRS;
using Ordering.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrders
{
    public record GetOrdersQuery<PaginationRequest>(PaginationRequest Pagination) : IQuery<GetOrdersResult>;
    public record GetOrdersResult(IEnumerable<OrderDto> OrderDtos);
}
