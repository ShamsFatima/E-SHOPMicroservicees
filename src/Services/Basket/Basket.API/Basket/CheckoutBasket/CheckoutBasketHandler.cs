using Basket.API.Data;
using Basket.API.Dtos;
using BuildingBlocks.CQRS;
using BuildingBlocks.Messaging.Events;
using FluentValidation;
using Mapster;
using MassTransit;
using System.Windows.Input;

namespace Basket.API.Basket.CheckoutBasket
{
    public record CheckoutBasketCommand(BasketCheckoutDtos BasketCheckoutDto) :
        ICommand<CheckoutBasketResult>;
    public record CheckoutBasketResult(bool IsSuccess);

    public class CheckoutBasketValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketValidator()
        {
            RuleFor(x => x.BasketCheckoutDto.UserName)
                .NotEmpty().WithMessage("UserName is required.");
            RuleFor(x => x.BasketCheckoutDto.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.");
            RuleFor(x => x.BasketCheckoutDto.TotalPrice)
                .GreaterThan(0).WithMessage("TotalPrice must be greater than zero.");
            RuleFor(x => x.BasketCheckoutDto.FirstName)
                .NotEmpty().WithMessage("FirstName is required.");
            RuleFor(x => x.BasketCheckoutDto.LastName)
                .NotEmpty().WithMessage("LastName is required.");
            RuleFor(x => x.BasketCheckoutDto.EmailAddress)
                .NotEmpty().WithMessage("EmailAddress is required.")
                .EmailAddress().WithMessage("EmailAddress must be a valid email.");
            RuleFor(x => x.BasketCheckoutDto.AddressLine)
                .NotEmpty().WithMessage("AddressLine is required.");
            RuleFor(x => x.BasketCheckoutDto.Country)
                .NotEmpty().WithMessage("Country is required.");
            RuleFor(x => x.BasketCheckoutDto.State)
                .NotEmpty().WithMessage("State is required.");
            RuleFor(x => x.BasketCheckoutDto.ZipCode)
                .NotEmpty().WithMessage("ZipCode is required.");
            RuleFor(x => x.BasketCheckoutDto.CardName)
                .NotEmpty().WithMessage("CardName is required.");
            RuleFor(x => x.BasketCheckoutDto.CardNumber)
                .NotEmpty().WithMessage("CardNumber is required.");
            RuleFor(x => x.BasketCheckoutDto.Expiration)
                .NotEmpty().WithMessage("Expiration is required.");
            RuleFor(x => x.BasketCheckoutDto.CVV)
                .NotEmpty().WithMessage("CVV is required.");
            RuleFor(x => x.BasketCheckoutDto.PaymentMethod)
                .GreaterThan(0).WithMessage("PaymentMethod must be a valid method.");
        }
    }
    public class CheckoutBasketCommandHandler(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetBasket(request.BasketCheckoutDto.UserName, cancellationToken);
            if (basket == null)
            {
                return new CheckoutBasketResult(false);
            }
            var eventMessage = request.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;
            await publishEndpoint.Publish(eventMessage, cancellationToken);
            await basketRepository.DeleteBasket(request.BasketCheckoutDto.UserName, cancellationToken);
            return new CheckoutBasketResult(true);
        }
    }
}


