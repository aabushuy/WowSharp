using RealmSrv.Repository;
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
            //TODO: as Transient!
            services.AddSingleton<IAccountRepository, AccountRepository>();


            return services;
        }
    }
}
