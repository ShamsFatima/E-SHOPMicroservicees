using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Catalog.API.Products.GetProducts;
using Marten;

namespace Catalog.API.Products.GetProductsByID
{
    public record GetProductByIDQuery(Guid Id) : IQuery<GetProductByIDResult>;
    public record GetProductByIDResult(Product Product);
    internal class GetProductByIDQueryHandler(IDocumentSession session) : IQueryHandler<GetProductByIDQuery, GetProductByIDResult>
    {
        public async Task<GetProductByIDResult> Handle(GetProductByIDQuery request, CancellationToken cancellationToken)
        {
          
            var product=await session.LoadAsync<Product>(request.Id,cancellationToken);
            if (product == null) {
                throw new ProductNotFoundException(request.Id);
            }
            return new GetProductByIDResult(product);

        }
    }
}
