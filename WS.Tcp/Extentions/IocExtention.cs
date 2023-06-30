using Microsoft.Extensions.DependencyInjection;

namespace WS.Tcp.Extentions
{
    public static class IocExtention
    {
        public static IServiceCollection AddTcpService(this IServiceCollection services)
        {
            return services;
        }
    }
}
