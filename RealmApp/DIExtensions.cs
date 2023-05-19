using Realm;

namespace RealmApp
{
    internal static class DIExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddRealmServices();
        }
    }
}
