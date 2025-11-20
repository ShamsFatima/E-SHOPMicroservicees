using Carter;
using JasperFx.CodeGeneration.Model;
using Mapster;
using MediatR;
using Basket.API.Dtos;

namespace Basket.API.Basket.CheckoutBasket
{
    public record CheckoutBasketRequest(BasketCheckoutDtos BasketCheckoutDto);
    public record CheckoutBasketResponse(bool IsSuccess);

    public class CheckoutBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/basket/checkout",
                 async (BasketCheckoutDtos dto, ISender sender) =>
                 {
                     var command = new CheckoutBasketCommand(dto);
                     var result = await sender.Send(command);

                     return Results.Ok(new CheckoutBasketResponse(result.IsSuccess));
                 })

            .WithName("Checkout Basket")
            .Produces<CheckoutBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Checkout Basket");
        }
    }
}

