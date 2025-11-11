using Basket.API.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository(IBasketRepository basketRepository,IDistributedCache distributedCache) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
           await basketRepository.DeleteBasket(userName, cancellationToken);
           await distributedCache.RemoveAsync(userName, cancellationToken);
            return true;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var cachebasket = await distributedCache.GetStringAsync(userName, cancellationToken);
            if (!string.IsNullOrEmpty(cachebasket))
                JsonSerializer.Deserialize<ShoppingCart>(cachebasket);
            var basket = await basketRepository.GetBasket(userName, cancellationToken);
            await distributedCache.SetStringAsync(userName, JsonSerializer.Serialize(basket),cancellationToken);
            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            await basketRepository.StoreBasket(cart, cancellationToken);
            await distributedCache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart), cancellationToken);
            return cart;
        }
    }
}
