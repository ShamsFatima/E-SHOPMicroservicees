using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task InitialiseDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context=scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.MigrateAsync().GetAwaiter().GetResult();
            await SeedAsync(context);
        }
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedCustomerAsync(context);   
            await SeedProductAsync(context);
            await SeedOrdersWithItemsAsync(context);    
        }
        private static async Task SeedCustomerAsync(ApplicationDbContext applicationDbContext)
        {
           if(!await applicationDbContext.Customers.AnyAsync())
            {
                await applicationDbContext.Customers.AddRangeAsync(InititalData.Customers);
                await applicationDbContext.SaveChangesAsync();
            }

        }
        private static async Task SeedProductAsync(ApplicationDbContext applicationDbContext)
        {
            if(!await applicationDbContext.Products.AnyAsync())
            {
                await applicationDbContext.Products.AddRangeAsync(InititalData.Products);
                await applicationDbContext.SaveChangesAsync();  
            }
        }
        private static async Task SeedOrdersWithItemsAsync(ApplicationDbContext applicationDbContext)
        {
            if(!await applicationDbContext.Orders.AnyAsync())
            {
                await applicationDbContext.Orders.AddRangeAsync(InititalData.OrdersWithItems);
                await applicationDbContext.SaveChangesAsync();
            }
        }
    }
}
