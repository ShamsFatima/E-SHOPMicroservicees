using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using Catalog.API.Models;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductsRequest(int? PageNumber=1,int? PageSize=10);
    public record GetProductResponse(IEnumerable<Product> Products);

    public class GetProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductsRequest request,ISender sender) =>
            {
                

                // Send query via MediatR
                var result = await sender.Send(new GetProductsQuery(request.PageNumber,request.PageSize));

                // Log what we got from DB
               

                // Create response (direct assignment to avoid Mapster confusion)
                var response = new GetProductResponse(result.products);

                return Results.Ok(response);

            })
            .WithName("GetProducts")
            .Produces<GetProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get all products")
            .WithDescription("Fetches all products from the database");
        }
    }
}
