using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace ManageDebts.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                // registra handlers, requests, notifications del ensamblado Application
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}
