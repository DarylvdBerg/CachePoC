using Microsoft.Extensions.DependencyInjection;

namespace Cache;

public static class ServiceCollectionExtensions
{
    public static void RegisterCache(this IServiceCollection services) {
        services.AddKeyedSingleton<ICacheService, MemoryCache>("memory");
        services.AddKeyedSingleton<ICacheService, RedisCache>("redis");
    }
}
