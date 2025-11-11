using Carter;
using Catalog.API.Models;
using Catalog.API.Products.GetProducts;
using Mapster;
using MediatR;

namespace Catalog.API.Products.GetProductsByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<Product> Product);
    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{categoryId}", async (string categoryId, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(categoryId));
                var response = result.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProductsByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get all GetProductsByCategory")
            .WithDescription("Fetches all ProductsByCategory from the database");

        }
    }
}
