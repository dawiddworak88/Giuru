using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Foundation.Media.Services.MediaServices;
using Seller.Web.Areas.Inventory.Repositories.Inventories;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.Definitions;
using Seller.Web.Areas.Orders.DomainModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.Services.BasketItems
{
    public class BasketItemsService : IBasketItemsService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMediaService _mediaService;

        public BasketItemsService(
            IInventoryRepository inventoryRepository,
            IMediaService mediaService)
        {
            _inventoryRepository = inventoryRepository;
            _mediaService = mediaService;
        }

        public async Task<IEnumerable<BasketItem>> GetBasketItemsAsync(string token, string language, IEnumerable<BasketItemRequestModel> items)
        {
            var basketItems = new List<BasketItem>();

            var productsOnStock = await _inventoryRepository.GetInventoryProductsAsync(
                token,
                language,
                null,
                PaginationConstants.DefaultPageIndex,
                PaginationConstants.DefaultPageSize,
                null);

            foreach (var item in items.OrEmptyIfNull())
            {
                var basketItem = new BasketItem
                {
                    ProductId = item.ProductId,
                    ProductSku = item.Sku,
                    ProductName = item.Name,
                    Quantity = item.Quantity,
                    StockQuantity = item.StockQuantity,
                    OutletQuantity = item.OutletQuantity,
                    PictureUrl = !string.IsNullOrWhiteSpace(item.ImageSrc) ? item.ImageSrc : (item.ImageId.HasValue ? _mediaService.GetMediaUrl(item.ImageId.Value, OrdersConstants.Basket.BasketProductImageMaxWidth) : null),
                    ExternalReference = item.ExternalReference,
                    MoreInfo = item.MoreInfo,
                    IsFromStock = item.IsFromStock,
                };

                if (item.IsFromStock)
                {
                    var productOnStock = productsOnStock.Data?.FirstOrDefault(x => x.ProductId == item.ProductId);

                    if (productOnStock is not null)
                    {
                        var quantity = item.Quantity + item.StockQuantity + item.OutletQuantity;

                        if (productOnStock.AvailableQuantity > quantity)
                        {
                            basketItem.Quantity = 0;
                            basketItem.StockQuantity = quantity;
                        }
                        else
                        {
                            basketItem.StockQuantity = productOnStock.AvailableQuantity;
                            basketItem.Quantity = quantity - productOnStock.AvailableQuantity;
                        }
                    }
                }

                basketItems.Add(basketItem);
            }

            return basketItems;
        }
    }
}
