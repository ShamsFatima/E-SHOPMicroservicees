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
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasConversion(
                customerId => customerId.Value,
                dbId => CustomerId.Of(dbId)); // Fixed the lambda parameter name to match the context
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Email).IsRequired().HasMaxLength(200);
            builder.HasIndex(c => c.Email).IsUnique();
        }
    }
}
