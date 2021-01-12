using Basket.Api.v1.Areas.Baskets.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Basket.Api.v1.Areas.Baskets.Repositories
{
    public class RedisBasketRepository : IBasketRepository
    {
        private readonly ConnectionMultiplexer redis;
        private readonly IDatabase database;

        public RedisBasketRepository(ConnectionMultiplexer redis)
        {
            this.redis = redis;
            this.database = redis.GetDatabase();
        }

        public async Task<BasketModel> GetBasketAsync(Guid id)
        {
            var data = await this.database.StringGetAsync(id.ToString());

            if (data.IsNullOrEmpty)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<BasketModel>(data);
        }

        public async Task<BasketModel> UpdateBasketAsync(BasketModel basket)
        {
            var created = await this.database.StringSetAsync(basket.Id.ToString(), JsonConvert.SerializeObject(basket));

            if (!created)
            {
                throw new Exception("Could not create the basket");
            }

            return await GetBasketAsync(basket.Id.Value);
        }
    }
}
