using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HiringService.Application.Cache;

public static class DistributedCacheExtensions
{
    public static async Task SetRecordAsync<T>(this IDistributedCache cache,
        string recordId,
        T data,
        TimeSpan? absoluteExpireTime = null,
        TimeSpan? unusedExpireTime = null)
    {
        if (cache is null) return;

        var options = new DistributedCacheEntryOptions();

        options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(3);
        options.SlidingExpiration = unusedExpireTime;

        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(recordId, jsonData, options);
    }

    public static async Task<T?> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
    {
        if (cache is null) return default(T);

        var jsonData = await cache.GetStringAsync(recordId);

        if (jsonData is null) return default(T);

        return JsonSerializer.Deserialize<T>(jsonData);
    }

    public static async Task RemoveRecordAsync(this IDistributedCache cache, string key)
    {
        if (cache is null) return;

        await cache.RemoveAsync(key);
    }
}
