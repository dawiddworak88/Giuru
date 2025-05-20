using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.Definitions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Giuru.IntegrationTests.Helpers
{
    public static class DataHelper
    {
        public static async Task<PagedResults<IEnumerable<T>>> GetDataAsync<T>(
            Func<Task<PagedResults<IEnumerable<T>>>> fetchData,
            Func<T, bool> condition)
        {
            int timeoutInSeconds = LoopTiming.TimeoutInSeconds;
            int elapsedSeconds = LoopTiming.ElapsedSeconds;

            while (elapsedSeconds < timeoutInSeconds)
            {
                var getResults = await fetchData();

                if (getResults.Data.Any(condition))
                {
                    return getResults;
                }

                await Task.Delay(LoopTiming.Delay);
                elapsedSeconds++;
            }

            return null;
        }
    }
}
