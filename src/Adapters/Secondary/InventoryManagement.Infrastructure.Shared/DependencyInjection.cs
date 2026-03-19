using InventoryManagement.Domain.Ports.Cache;
using InventoryManagement.Domain.Ports.Context;
using InventoryManagement.Infrastructure.Shared.Caching;
using InventoryManagement.Infrastructure.Shared.Context;
using InventoryManagement.Infrastructure.Shared.Options;
using InventoryManagement.Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Infrastructure.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var cacheOptions = configuration
            .GetSection(CacheOptions.SectionName)
            .Get<CacheOptions>() ?? new CacheOptions();

        services.Configure<CacheOptions>(
            configuration.GetSection(CacheOptions.SectionName));

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserContext, CurrentUserContext>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = cacheOptions.Redis.ConnectionString;
        });

        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = cacheOptions.Hybrid.MaximumPayloadBytes;
            options.MaximumKeyLength = cacheOptions.Hybrid.MaximumKeyLength;
            options.DefaultEntryOptions = new Microsoft.Extensions.Caching.Hybrid.HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromSeconds(cacheOptions.Hybrid.LocalCacheExpirationSeconds),
                Expiration = TimeSpan.FromSeconds(cacheOptions.Hybrid.DistributedCacheExpirationSeconds)
            };
        });

        services.AddScoped<IAppCache, AppCache>();

        return services;
    }
}