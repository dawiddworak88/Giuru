using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foundation.Extensions.Services.Cache
{
    public interface ICacheService
    {
        Task<IEnumerable<T>> GetOrSetAsync<T>(string cacheKey, Func<Task<IEnumerable<T>>> fetchDataFunc, TimeSpan? cacheDuration = null) where T : class;
        Task InvalidateAsync(string cacheKey);
    }
}