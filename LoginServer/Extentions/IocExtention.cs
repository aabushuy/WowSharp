using LoginServer.Network;
using LoginServer.Repository;
using LoginServer.Services;
using System.Reflection;
using WS.Tcp;

namespace LoginServer.Extentions
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
