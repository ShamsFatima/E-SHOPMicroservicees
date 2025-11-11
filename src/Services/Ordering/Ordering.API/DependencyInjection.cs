using Carter;

namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // Add API services here (e.g., controllers, Swagger, CORS, etc.)
            services.AddCarter();
            return services;
        }  
        public static WebApplication UseAPiServices(this WebApplication webApplication)
        {
            // Configure API middleware here (e.g., routing, authentication, etc.)
            webApplication.MapCarter();
            return webApplication;
        }
    }
}
