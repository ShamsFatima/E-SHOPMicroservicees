using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Catalog.API.Products.GetProductsByID;
using Catalog.API.Products.UpdateProduct;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : IQuery<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);
    public class DeleteProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");
            
        }
    }
    internal class DeleteProductCommandHandler(IDocumentSession session)
        : IQueryHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            session.Delete<Product>(request.Id);
            await session.SaveChangesAsync(cancellationToken);
            return new DeleteProductResult(true);
        }
    }
}
