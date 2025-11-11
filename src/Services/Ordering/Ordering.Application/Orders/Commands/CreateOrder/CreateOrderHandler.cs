using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler(IApplicationDbContext applicationDbContext) : 
        ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = CreateNewOrder(request.OrderDto);
            applicationDbContext.Orders.Add(order);
            await applicationDbContext.SaveChangesAsync(cancellationToken);
            return new CreateOrderResult(order.Id); 
        }
        private Order CreateNewOrder(OrderDto orderDto)
        {
            var shippingAddress= Address.Of(
                orderDto.ShippingAddress.FirstName,
                orderDto.ShippingAddress.LastName,
                orderDto.ShippingAddress.EmailAddress, 
                orderDto.ShippingAddress.AddressLine,
                orderDto.ShippingAddress.Country,
                orderDto.ShippingAddress.State,
                orderDto.ShippingAddress.ZipCode);
            var billingAddress=Address.Of(orderDto.ShippingAddress.FirstName,
                orderDto.ShippingAddress.LastName,
                orderDto.ShippingAddress.EmailAddress,
                orderDto.ShippingAddress.AddressLine,
                orderDto.ShippingAddress.Country,
                orderDto.ShippingAddress.State,
                orderDto.ShippingAddress.ZipCode);
            var newOrder = Order.Create(
                id: OrderId.Of(Guid.NewGuid()),
                customerId: CustomerId.Of(orderDto.CustomerId),
                orderName: OrderName.Of(orderDto.OrderName),
                shippingAddress: shippingAddress,
                billingAddress: billingAddress,
                payment: Payment.Of(
                    orderDto.Payment.CardName,
                    orderDto.Payment.CardNumber,
                    orderDto.Payment.Expiration,
                    orderDto.Payment.Cvv,
                    orderDto.Payment.PaymentMethod));
            foreach (var item in orderDto.OrderItems)
            {
                newOrder.Add(
                      ProductId.Of(item.ProductId),
                      item.Quantity,
                      item.Price);
            }
            return newOrder;
        }
    }
}
