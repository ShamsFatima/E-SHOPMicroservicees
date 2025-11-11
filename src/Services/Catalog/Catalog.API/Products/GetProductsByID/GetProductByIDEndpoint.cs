using Carter;
using Catalog.API.Models;
using Catalog.API.Products.GetProducts;
using Mapster;
using MediatR;

namespace Catalog.API.Products.GetProductsByID
{
    public record GetProductByIdResponse(Product Product);
    public class GetProductByIDEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}",async (Guid id,ISender sender) =>
            {
                var result=await sender.Send(new GetProductByIDQuery(id));
                var response=result.Adapt<GetProductByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProductById")
            .Produces<GetProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get all products by ID")
            .WithDescription("Fetches all products by ID from the database");
        }
    }
}
