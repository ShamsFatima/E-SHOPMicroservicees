using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.CreateOrder;
using System.Threading.Tasks;

namespace Ordering.API.Endpoints
{
    public record CreateOrderRequest(OrderDto OrderDto);
    public record CreateOrderResponse(Guid Id);

    public class CreateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
            {
                // ✅ Correct: map to CreateOrderCommand, not CreateOrderRequest
                var command = request.Adapt<CreateOrderCommand>();

                // ✅ Send command to MediatR (command implements IRequest)
                var result = await sender.Send(command);

                var response = result.Adapt<CreateOrderResponse>();

                return Results.Created($"/orders/{response.Id}", response);
            })
            .WithName("CreateOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Creates a new order")
            .WithDescription("Creates a new order with the provided details.");
        }
    }
}
