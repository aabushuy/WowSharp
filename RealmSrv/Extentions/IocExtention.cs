using RealmSrv.Network;
using RealmSrv.Repository;
using RealmSrv.Services;
using System.Reflection;
using WS.Tcp;

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
            services.AddSingleton<ITcpServer, RealmTcpServer>();

            services.AddTransient<IAuthEngine, AuthEngine>();
            services.AddTransient<IRealmRepository, RealmRepository>();
            
            //TODO: as Transient!
            services.AddSingleton<IAccountRepository, AccountRepository>();

            return services;
        }
    }
}
