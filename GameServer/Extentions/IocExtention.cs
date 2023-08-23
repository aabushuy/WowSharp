using GameServer.Factory;
using GameServer.Network;
using System.Reflection;
using WS.Tcp;
using WS.Tcp.Extentions;

namespace GameServer.Extentions
{
    internal static class IocExtention
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
            => services
                .AddTcpService()
                .AddLocalServices()                
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        private static IServiceCollection AddLocalServices(this IServiceCollection services)
        {
            services.AddSingleton<ITcpServer, GameTcpServer>();

            services.AddTransient<IRequestFactory, RequestFactory>();

            return services;
        }
    }
}
