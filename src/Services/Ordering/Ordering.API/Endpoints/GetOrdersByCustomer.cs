using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.API.Endpoints
{
    public record GetOrdersByCustomersRequest(Guid CustomerId);
    public record GetOrdersByCustomersResponse(IEnumerable<OrderDto> OrderDtos);

    public class GetOrdersByCustomer : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/customer/{customerId}", async (Guid customerId, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByCustomerQuery(customerId));
                var response = result.Adapt<GetOrdersByCustomersResponse>();
                return Results.Ok(response);
            })
            .WithName("GetOrdersByCustomers")
            .Produces<GetOrdersByCustomersResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Orders by Customers");
        }
    }
}
