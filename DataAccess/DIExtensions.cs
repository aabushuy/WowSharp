using DataAccess.Repositories;
using DataAccess.Repositories.Instances;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class DIExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddTransient<IAccountInfoRepository, AccountInfoRepository>();

            return services;
        }
    }
}
