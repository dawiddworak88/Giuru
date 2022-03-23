using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Shared.DomainModels.Baskets;
using Foundation.Extensions.ExtensionMethods;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Baskets
{
    public class BasketService : IBasketService
    {
        private readonly LinkGenerator linkGenerator;
        private readonly IBasketRepository basketRepository;

        public BasketService(
            LinkGenerator linkGenerator,
            IBasketRepository basketRepository)
        {
            this.linkGenerator = linkGenerator;
            this.basketRepository = basketRepository;
        }

        public async Task<IEnumerable<BasketItem>> GetBasketAsync(Guid? basketId, string token, string language)
        {
            var existingBasket = await this.basketRepository.GetBasketById(token, language, basketId);
            if (existingBasket is not null)
            {
                var productIds = existingBasket.Items.OrEmptyIfNull().Select(x => x.ProductId.Value);
                if (productIds.OrEmptyIfNull().Any())
                {
                    var basketItems = existingBasket.Items.OrEmptyIfNull().Select(x => new BasketItem
                    {
                        ProductId = x.ProductId,
                        ProductUrl = this.linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                        Name = x.ProductName,
                        Sku = x.ProductSku,
                        Quantity = x.Quantity,
                        StockQuantity = x.StockQuantity,
                        OutletQuantity = x.OutletQuantity,
                        TotalQuantity = x.TotalQuantity,
                        ExternalReference = x.ExternalReference,
                        ImageSrc = x.PictureUrl,
                        ImageAlt = x.ProductName,
                        DeliveryFrom = x.DeliveryFrom,
                        DeliveryTo = x.DeliveryTo,
                        MoreInfo = x.MoreInfo
                    });

                    return basketItems;
                }
            }

            return default;
        }
    }
}
