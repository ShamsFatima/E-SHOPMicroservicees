using BuildingBlocks.CQRS;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrdersByName
{
    public class GetOrdersByNameHandler(IApplicationDbContext applicationDbContext) :
                IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery request, CancellationToken cancellationToken)
        {
            var orders = await applicationDbContext.Orders.Include(o => o.OrderItems)
                .Where(o => o.OrderName.Value.Contains(request.Name))
                .OrderBy(o => o.OrderName.Value)
                .ToListAsync(cancellationToken);
            //var orderDtos= ProjectToOrdersDto(orders);
            return new GetOrdersByNameResult(orders.ToOrderDtoList());

        }
        private List<OrderDto> ProjectToOrdersDto(List<Order> orders)
        {
            List<OrderDto> result = new();
            foreach (var order in orders) 
            {
                var orderDto = new OrderDto(
                    Id: order.Id.Value,
                    CustomerId: order.CustomerId.Value,
                    OrderName: order.OrderName.Value,
                    ShippingAddress: new AddressDto(
                        order.ShippingAddress.FirstName,
                        order.ShippingAddress.LastName,
                        order.ShippingAddress.EmailAddrress,
                        order.ShippingAddress.AddressLine,
                        order.ShippingAddress.Country,
                        order.ShippingAddress.State,
                        order.ShippingAddress.ZipCode
                        ),
                    BillingAddress: new AddressDto(
                        order.BillingAddress.FirstName,
                        order.BillingAddress.LastName,
                        order.BillingAddress.EmailAddrress,
                        order.BillingAddress.AddressLine,
                        order.BillingAddress.Country,
                        order.BillingAddress.State,
                        order.BillingAddress.ZipCode),
                    Payment: new PaymentDto(
                        order.Payment.CardName,
                        order.Payment.CardNumber,
                        order.Payment.Expiration,
                        order.Payment.CVV,
                        order.Payment.PaymentMethod),
                    Status: order.Status,
                    OrderItems: order.OrderItems.Select(oi => new OrderItemDto(
                        OrderId:oi.OrderId.Value,
                        ProductId: oi.ProductId.Value,
                        Quantity: oi.Quantity,
                        Price: oi.Price
                        )).ToList()
                    );
                result.Add(orderDto);
            }
            return result;
        }
    }
}
