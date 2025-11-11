using Ordering.Domain.Abstractions;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Models
{
    public class Product:Entity<ProductId>
    {
        public string Name { get; private set; } = string.Empty;    
        public decimal Price { get; private set; } = default!;
        public static Product Create(string name, decimal price)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
            }
            var product = new Product
            {
                Id = ProductId.Of(Guid.NewGuid()),
                Name = name,
                Price = price
            };
            return product;
        }   
    }
}
