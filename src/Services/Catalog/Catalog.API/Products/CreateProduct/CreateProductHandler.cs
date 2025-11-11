using BuildingBlocks.CQRS;
using MediatR;
using Catalog.API.Models;
using Marten;
using FluentValidation;

namespace Catalog.API.Products.CreateProduct
{
    // ✅ Consistent naming: use "Id" in result as well
    public record CreateProductCommand(
        string Name,
        List<string> Category,
        string Description,
        string ImageFile,
        decimal? Price
    ) : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid ID);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image file is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("price must be greater than 0");
        }
    }
    internal class CreateProductHandler(IDocumentSession session)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
           
            // ✅ Create new Product
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            // ✅ Store in Marten (auto-generates Guid Id)
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            // ✅ Return generated Id
            return new CreateProductResult(product.ID);
        }
    }
}
