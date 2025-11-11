using Basket.API.Exceptions;
using Basket.API.Models;
using Marten;

namespace Basket.API.Data
{
    public class BasketRepository(IDocumentSession documentSession) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            documentSession.Delete<ShoppingCart>(userName);
            await documentSession.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await documentSession.LoadAsync<ShoppingCart>(userName, cancellationToken);
            return basket is null ? throw new BasketNotFoundException(userName) : basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            documentSession.Store(cart);
            await documentSession.SaveChangesAsync(cancellationToken);
            return cart;
        }
    }
}
