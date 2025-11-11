using Carter;
using Mapster;
using MediatR;
using Ordering.API.Orders.Commands.CreateOrder;
using Ordering.Application;
using Ordering.Application.Dtos;
using System.Threading.Tasks;
namespace Ordering.API.Endpoints
{
    public record CreateOrderRequest(OrderDto OrderDto);
    public record CreateOrderResponse(Guid Id);
    public class CreateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders",async (CreateOrderRequest request,ISender sender) =>
            {
               var command=request.Adapt<CreateOrderCommand>(); 
                var result=await sender.Send(command);
                var response= result.Adapt<CreateOrderResponse>();
                return Results.Created($"/orders/{response.Id}", response); 

            }).WithName("CreateProduct")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)   
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Creates a new order")
            .WithDescription("Creates a new order with the provided details."); 

        }
    }
}
