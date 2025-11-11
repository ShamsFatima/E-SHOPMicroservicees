using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(
                    id => id.Value,                    // When saving → Guid
                    value => OrderItemId.Of(value)     // When reading → Value Object
                )
                .IsRequired();

            // 🔹 Foreign Key: ProductId (One Product → Many OrderItems)
            builder.HasOne<Product>()                 // Each OrderItem has one Product
                .WithMany()                           // One Product has many OrderItems
                .HasForeignKey(x => x.ProductId);      // FK column in OrderItem table

            // 🔹 Required Properties
            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.Price)
                .IsRequired();
        }
    }
}
