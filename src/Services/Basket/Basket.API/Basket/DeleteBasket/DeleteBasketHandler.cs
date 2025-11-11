using Basket.API.Data;
using BuildingBlocks.CQRS;
using FluentValidation;
using System.Windows.Input;

namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketCommand(string userName) : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool IsSuccess);

    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.userName).NotEmpty().WithMessage("UserName is required");
        }
        public class DeleteBasketHandler(IBasketRepository basketRepository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>

        {
            public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
            {
                await basketRepository.DeleteBasket(request.userName, cancellationToken).ConfigureAwait(false);
                return new DeleteBasketResult(true);
            }
        }
    }
}
