using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(builder => builder.Id);
            builder.Property(x => x.Id)
                .HasConversion(
                    id => id.Value,                    // Save as Guid
                    value => OrderId.Of(value)         // Read as Value Object
                )
                .IsRequired();
            builder.HasOne<Customer>().WithMany()
                .HasForeignKey(o => o.CustomerId)
                .IsRequired();  
            builder.HasMany(o=>o.OrderItems).WithOne()
                .HasForeignKey(oi => oi.OrderId)
                .IsRequired();
            builder.ComplexProperty(builder => builder.OrderName, on =>
            {
                on.Property(on => on.Value).HasColumnName("OrderName").IsRequired().HasMaxLength(200);
            });
            builder.ComplexProperty(x => x.BillingAddress, addressBuilder =>
            {
                addressBuilder.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
                addressBuilder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
                addressBuilder.Property(a => a.EmailAddrress).HasMaxLength(255).IsRequired();
                addressBuilder.Property(a => a.AddressLine).HasMaxLength(200).IsRequired();
                addressBuilder.Property(a => a.Country).HasMaxLength(100).IsRequired();
                addressBuilder.Property(a => a.State).HasMaxLength(100).IsRequired();
                addressBuilder.Property(a => a.ZipCode).HasMaxLength(10).IsRequired();
            });

           
            builder.ComplexProperty(x => x.ShippingAddress, addressBuilder =>
            {
                addressBuilder.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
                addressBuilder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
                addressBuilder.Property(a => a.EmailAddrress).HasMaxLength(255).IsRequired();
                addressBuilder.Property(a => a.AddressLine).HasMaxLength(200).IsRequired();
                addressBuilder.Property(a => a.Country).HasMaxLength(100).IsRequired();
                addressBuilder.Property(a => a.State).HasMaxLength(100).IsRequired();
                addressBuilder.Property(a => a.ZipCode).HasMaxLength(10).IsRequired();
            });


            // 🔹 Complex Property: Payment (Value Object)
            builder.ComplexProperty(x => x.Payment, paymentBuilder =>
            {
                paymentBuilder.Property(p => p.CardName).HasMaxLength(100).IsRequired();
                paymentBuilder.Property(p => p.CardNumber).HasMaxLength(16).IsRequired();
                paymentBuilder.Property(p => p.Expiration).HasMaxLength(5).IsRequired();
                paymentBuilder.Property(p => p.CVV).HasMaxLength(4).IsRequired();
            });
            

            // 🔹 TotalPrice column
            builder.Property(x => x.TotalPridce)
                .IsRequired();

        }
    }
}
