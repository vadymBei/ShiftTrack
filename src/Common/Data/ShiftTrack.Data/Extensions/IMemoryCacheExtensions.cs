using Microsoft.Extensions.Caching.Memory;

namespace ShiftTrack.Data.Extensions
{
    public static class IMemoryCacheExtensions
    {
        public static MemoryCacheEntryOptions DefaultExpiration => new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        };

        public static async Task<T> GetOrCreateAsync<T>(
            this IMemoryCache memoryCache,
            string key,
            Func<Task<T>> factory,
            MemoryCacheEntryOptions? memoryCacheEntryOptions = null)
        {
            if (memoryCache.TryGetValue(key, out T cachedData))
            {
                return cachedData;
            }

            var data = await factory();

            memoryCache.Set(
                key,
                data,
                memoryCacheEntryOptions ?? DefaultExpiration);

            return data;
        }
    }
}
