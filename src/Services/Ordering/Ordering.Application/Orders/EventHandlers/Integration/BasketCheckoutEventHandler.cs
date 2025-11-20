using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.CreateOrder;
namespace Ordering.Application.Orders.EventHandlers.Integration
{
    public class BasketCheckoutEventHandler(ISender sender,ILogger<BasketCheckoutEventHandler> logger) : 
        IConsumer<BasketCheckoutEvent>
    {
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            logger.LogInformation("Integration Event Handled:{IntegrationEvent}",context);
            var command=MapToCreateOrderCommand(context.Message);
            await sender.Send(command);
        }
        private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
        {
            var addressDto = new AddressDto(
                message.FirstName,
                message.LastName,
                message.EmailAddress,
                message.AddressLine,
                message.Country,
                message.State,
                message.ZipCode
            );

            var paymentDto = new PaymentDto(
                message.CardName,
                message.CardNumber,
                message.Expiration,
                message.CVV,
                message.PaymentMethod
            );

            var orderId = Guid.NewGuid();

            var orderDto = new OrderDto(
                Id: orderId,
                CustomerId: message.CustomerId,
                OrderName: message.OrderName,
                ShippingAddress: addressDto,
                BillingAddress: addressDto,
                Payment: paymentDto,
                Status: Ordering.Domain.Enums.OrderStatus.Pending,

                OrderItems: [
                    new OrderItemDto(
                orderId,
                Guid.Parse("A5ADF762-6F8B-42BF-BA43-1619636EC243"),
                2,
                100
            ),

            new OrderItemDto(
                orderId,
                Guid.Parse("64F690C5-3549-4D6A-BBF1-6D8B1F55AB99"),
                1,
                90
            )
                ]
            );

            return new CreateOrderCommand(orderDto);
        }

    }
}
