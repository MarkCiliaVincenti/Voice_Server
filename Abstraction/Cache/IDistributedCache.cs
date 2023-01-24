using Microsoft.Extensions.Caching.Distributed;

namespace Abstraction.Cache;

public interface IDistributedCache<TCacheItem, TCacheKey> where TCacheItem : class
{
    Task<TCacheItem> GetAsync(
        TCacheKey key,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default(CancellationToken));

    Task SetAsync(
        TCacheKey key,
        TCacheItem value,
        DistributedCacheEntryOptions options = null,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default(CancellationToken));
}