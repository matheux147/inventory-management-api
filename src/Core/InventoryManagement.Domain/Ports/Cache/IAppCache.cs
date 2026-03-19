namespace InventoryManagement.Domain.Ports.Cache;

public interface IAppCache
{
    ValueTask<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        CacheEntryOptions? options = null,
        IReadOnlyCollection<string>? tags = null,
        CancellationToken cancellationToken = default);

    ValueTask RemoveAsync(
        string key,
        CancellationToken cancellationToken = default);

    ValueTask RemoveByTagAsync(
        string tag,
        CancellationToken cancellationToken = default);
}
