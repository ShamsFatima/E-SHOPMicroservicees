using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class ApplicationDbContext:DbContext,IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers=>Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        DbSet<Customer> IApplicationDbContext.Customers { get => Customers; set => throw new NotImplementedException(); }
        DbSet<Product> IApplicationDbContext.Products { get => Products; set => throw new NotImplementedException(); }
        DbSet<Order> IApplicationDbContext.Orders { get => Orders; set => throw new NotImplementedException(); }
        DbSet<OrderItem> IApplicationDbContext.OrderItems { get => OrderItems; set => throw new NotImplementedException(); }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }   
    }
}
