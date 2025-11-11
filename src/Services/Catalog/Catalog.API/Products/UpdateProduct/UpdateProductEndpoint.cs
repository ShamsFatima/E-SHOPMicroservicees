using Carter;
using Catalog.API.Products.GetProducts;
using Mapster;
using MediatR;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateproductRequest(Guid Id,
      string Name,
      List<string> Category,
      string Description,
      string ImageFile,
      decimal? Price);
    public record UpdateproductResponse(bool IsSuccess);

    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products",async (UpdateproductRequest request,ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command);
                var respone=result.Adapt<UpdateproductResponse>();
                return Results.Ok(respone);

            })
            .WithName("UpdateProductById")
            .Produces<UpdateproductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Product")
            .WithDescription("Update Product");
        }
    }
}
