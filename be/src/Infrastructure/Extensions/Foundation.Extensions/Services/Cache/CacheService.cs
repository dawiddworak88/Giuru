using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Extensions.Services.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _cache;

        public CacheService(
            ConnectionMultiplexer redis)
        {
            _cache = redis.GetDatabase();
        }

        public async Task<IEnumerable<T>> GetOrSetAsync<T>(string cacheKey, Func<Task<IEnumerable<T>>> fetchDataFunc, TimeSpan? cacheDuration = null) where T : class
        {
            var cachedData = await _cache.StringGetAsync(cacheKey);

            if (cachedData.HasValue)
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(cachedData);
            }

            if (fetchDataFunc is null)
            {
                return Enumerable.Empty<T>();
            }

            var data = await fetchDataFunc();

            if (data is not null && data.Any())
            {
                var serializedData = JsonConvert.SerializeObject(data);


                await _cache.StringSetAsync(cacheKey, serializedData, cacheDuration ?? null);
            }

            return data;
        }

        public async Task InvalidateAsync(string cacheKey)
        {
            if (await _cache.KeyExistsAsync(cacheKey))
            {
                await _cache.KeyDeleteAsync(cacheKey);
            }
        }
    }
}
