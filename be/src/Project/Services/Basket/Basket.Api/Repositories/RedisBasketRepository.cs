using Basket.Api.RepositoriesModels;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public class RedisBasketRepository : IBasketRepository
    {
        private readonly IDatabase database;

        public RedisBasketRepository(ConnectionMultiplexer redis)
        {
            this.database = redis.GetDatabase();
        }

        public async Task<BasketRepositoryModel> GetBasketAsync(Guid id)
        {
            var data = await this.database.StringGetAsync(id.ToString());

            if (data.IsNullOrEmpty)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<BasketRepositoryModel>(data);
        }

        public async Task<BasketRepositoryModel> UpdateBasketAsync(BasketRepositoryModel basket)
        {
            var created = await this.database.StringSetAsync(basket.Id.ToString(), JsonConvert.SerializeObject(basket));

            if (!created)
            {
                throw new Exception("Could not create the basket");
            }

            return await GetBasketAsync(basket.Id.Value);
        }

        public async Task<bool> DeleteBasketAsync(Guid? basketId)
        {
            if (basketId.HasValue)
            {
                return await this.database.KeyDeleteAsync(basketId.Value.ToString());
            }

            return false;
        }
    }
}
