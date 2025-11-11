using Basket.API.Basket.GetBasket;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Basket.DeleteBasket
{
    //public record DeleteBasketRequest(string userName);
    public record DeleteBasketResponse(bool isSuccess);
    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName,ISender sender) =>
            {
               var result=await sender.Send(new DeleteBasketCommand(userName));
                var response=result.Adapt<DeleteBasketResponse>();
                return Results.Ok(response);
            })
           .WithName("DeleteBasketEndpoint")
           .Produces<GetBasketEndpoint>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .WithSummary("DeleteBasketEndpoint")
           .WithDescription("DeleteBasketEndpoint from the database");
        }
    }
}
