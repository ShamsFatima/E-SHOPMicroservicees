using BuildingBlocks.Behaviours;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddAppplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidationBehaviours<,>));
                config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            });   
            

            return services;
        }
    }
}
