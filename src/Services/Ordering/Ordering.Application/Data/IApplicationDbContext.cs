using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Order> Orders { get; set; }   
        DbSet<OrderItem> OrderItems { get; set; }   
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
