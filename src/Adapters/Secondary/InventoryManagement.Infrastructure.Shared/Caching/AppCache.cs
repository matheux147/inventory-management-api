using InventoryManagement.Domain.Ports.Cache;
using Microsoft.Extensions.Caching.Hybrid;

namespace InventoryManagement.Infrastructure.Shared.Caching;

public sealed class AppCache(HybridCache hybridCache) : IAppCache
{
    private readonly HybridCache _hybridCache = hybridCache;

    public async ValueTask<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        CacheEntryOptions? options = null,
        IReadOnlyCollection<string>? tags = null,
        CancellationToken cancellationToken = default)
    {
        var entryOptions = options is null
            ? null
            : new HybridCacheEntryOptions
            {
                Expiration = options.Expiration,
                LocalCacheExpiration = options.LocalCacheExpiration
            };

        return await _hybridCache.GetOrCreateAsync(
            key,
            async cancel => await factory(cancel),
            options: entryOptions,
            tags: tags,
            cancellationToken: cancellationToken);
    }

    public ValueTask RemoveAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        return _hybridCache.RemoveAsync(key, cancellationToken);
    }

    public ValueTask RemoveByTagAsync(
        string tag,
        CancellationToken cancellationToken = default)
    {
        return _hybridCache.RemoveByTagAsync(tag, cancellationToken);
    }
}