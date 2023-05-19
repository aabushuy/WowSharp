using DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Realm.Services;
using System.Reflection;

namespace Realm
{
    public static class DIExtensions
    {
        public static IServiceCollection AddRealmServices(this IServiceCollection services)
        {
            services
                .AddDataAccess()
                .AddLocalServices()
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }

        private static IServiceCollection AddLocalServices(this IServiceCollection services)
        {
            services.AddTransient<IConnectionListener, ConnectionListener>();
            
            services.AddTransient<IAuthEngine, AuthEngine>();

            return services;
        }
    }
}
