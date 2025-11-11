using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using Discount.Grpc.Protos;
using FluentValidation;
using System.Windows.Input;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart ShoppingCart):ICommand<StoreBasketResult>;
    public record StoreBasketResult(string userName);
    public class StoreBasketHandler : AbstractValidator<StoreBasketCommand> 
    {
        public StoreBasketHandler() 
        {
            RuleFor(x=>x.ShoppingCart).NotNull().NotEmpty().WithMessage("Cart can't be null");
            RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("Username is required");
        }

    }
    public class StoreBasketCommandHandler(
    IBasketRepository basket,
    DiscountProtoService.DiscountProtoServiceClient discountProtoService)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
        {
            await DeductDiscount(request.ShoppingCart, cancellationToken);
            await basket.StoreBasket(request.ShoppingCart, cancellationToken);
            return new StoreBasketResult(request.ShoppingCart.UserName);
        }

        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var coupon = await discountProtoService.GetDiscountAsync(
                    new GetDiscountRequest { ProductName = item.ProductName },
                    cancellationToken: cancellationToken);

                item.Price -= (decimal)coupon.Amount;

            }
        }
    }


}
