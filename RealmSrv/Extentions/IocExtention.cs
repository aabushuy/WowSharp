using RealmSrv.Repository;
using RealmSrv.Services;
using System.Reflection;

namespace RealmSrv.Extentions
{
    internal static class IocExtention
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
            => services
                .AddLocalServices()
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        private static IServiceCollection AddLocalServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthEngine, AuthEngine>();            
            services.AddTransient<IRealmRepository, RealmRepository>();
            
            //TODO: as Transient!
            services.AddSingleton<IAccountRepository, AccountRepository>();

            return services;
        }
    }
}
