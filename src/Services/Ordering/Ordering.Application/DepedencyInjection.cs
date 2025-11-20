using BuildingBlocks.Behaviours;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddAppplicationServices(
            this IServiceCollection services,IConfiguration configuration)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidationBehaviours<,>));
                config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            });
            services.AddFeatureManagement();
            services.AddMessageBroker(configuration,Assembly.GetExecutingAssembly());


            return services;
        }
    }
}
