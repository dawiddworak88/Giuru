using Basket.Api.v1.Areas.Baskets.Repositories;
using Basket.Api.v1.Areas.Baskets.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.Api.v1.Areas.Baskets.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterBasketDependencies(this IServiceCollection services)
        {
            services.AddScoped<IBasketRepository, RedisBasketRepository>();
            services.AddScoped<IBasketService, BasketService>();
        }
    }
}