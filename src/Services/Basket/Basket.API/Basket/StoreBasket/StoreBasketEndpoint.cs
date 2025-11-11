using Basket.API.Basket.GetBasket;
using Basket.API.Models;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart ShoppingCart);
    public record StoreBasketResponse(string userName);
    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async ([FromBody] StoreBasketRequest request, [FromServices] ISender sender) =>
            {
                var command = new StoreBasketCommand(request.ShoppingCart);
                var result = await sender.Send(command);
                return Results.Created($"/basket/{result.userName}", new StoreBasketResponse(result.userName));
            })
           .WithName("StoreBasketEndpoint")
           .Produces<GetBasketEndpoint>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .WithSummary("StoreBasketEndpoint")
           .WithDescription("StoreBasketEndpoint from the database");
        }
    }    
}

