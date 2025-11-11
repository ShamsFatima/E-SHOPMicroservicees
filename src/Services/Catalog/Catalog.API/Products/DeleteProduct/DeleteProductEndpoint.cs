using Carter;
using Catalog.API.Models;
using Catalog.API.Products.GetProducts;
using Mapster;
using MediatR;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async (Guid id,ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductCommand(id));
                var response =result.Adapt<DeleteProductCommand>();
                return Results.Ok(response);

            })
           .WithName("DeleteProducts")
           .Produces<GetProductResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .WithSummary("Delete all products")
           .WithDescription("Delete products from the database");
        }
    }
}
