using Cache.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cache
{
    public static class ServiceCollectionExtension
    {
        public static void AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddSingleton<ILocker, MemoryCacheManager>();
            services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
        }
    }
}