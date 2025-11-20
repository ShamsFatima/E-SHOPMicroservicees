using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.UpdateOrder;

namespace Ordering.API.Endpoints
{
    public record UpdateOrderRequest(OrderDto OrderDto);
    public record UpdateOrderResponse(bool IsSuccess);
    public class UpdateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/orders", async (UpdateOrderRequest request,ISender sender) =>
            {
                // Here you would typically update the order in your database
                // For demonstration, we will just return a mock response
               var command=request.Adapt<UpdateOrderCommand>();
                var order = await sender.Send(command);
                var orderDto = order.Adapt<UpdateOrderResponse>();
                return Results.Ok(orderDto);
            })
            .WithName("UpdateOrder")
            .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update an existing order")
            .WithDescription("Updates the details of an existing order in the system.");

        }
    }
}
