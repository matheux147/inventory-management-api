namespace InventoryManagement.Infrastructure.Shared.Options;

public sealed class CacheOptions
{
    public const string SectionName = "Cache";

    public RedisCacheOptions Redis { get; init; } = new();
    public HybridCacheOptions Hybrid { get; init; } = new();
}

public sealed class RedisCacheOptions
{
    public string ConnectionString { get; init; } = "redis:6379";
}

public sealed class HybridCacheOptions
{
    public int MaximumPayloadBytes { get; init; } = 1024 * 1024;
    public int MaximumKeyLength { get; init; } = 1024;
    public int LocalCacheExpirationSeconds { get; init; } = 20;
    public int DistributedCacheExpirationSeconds { get; init; } = 60;
}