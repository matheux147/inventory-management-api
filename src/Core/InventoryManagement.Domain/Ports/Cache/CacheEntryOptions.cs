namespace InventoryManagement.Domain.Ports.Cache;

public sealed record CacheEntryOptions(
    TimeSpan? Expiration = null,
    TimeSpan? LocalCacheExpiration = null);
