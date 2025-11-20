using BuildingBlocks.Pagination;
using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Queries.GetOrders;

namespace Ordering.API.Endpoints
{
    
    public record GetOrdersResponse(PaginationResult<OrderDto> Orders);
    public class GetOrders : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/orders", async ([AsParameters] PaginationRequest paginationRequest, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersQuery(paginationRequest));
                var response = result.Adapt<GetOrdersResponse>();
                return Results.Ok(response);
            })

             .WithName("Get Orders")
             .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Get Orders")
             .WithDescription("Get Orders");
        }
    }
}
