using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ShiftTrack.Core.Application.Data.Common.Exceptions
{
    public static class DistributedCacheExtensions
    {
        public static DistributedCacheEntryOptions DefaultExpiration => new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        };

        public static async Task<T> GetOrCreateAsync<T>(
            this IDistributedCache cache,
            string key,
            Func<Task<T>> factory,
            DistributedCacheEntryOptions? cacheOptions = null)
        {
            var cachedData = await cache.GetStringAsync(key);

            if (cachedData is not null)
            {
                return JsonConvert.DeserializeObject<T>(cachedData);
            }

            var data = await factory();

            await cache.SetStringAsync(
                key,
                JsonConvert.SerializeObject(data),
                cacheOptions ?? DefaultExpiration);

            return data;
        }
    }
}
