using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Services.MediaServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Media.ApiRequestModels;
using Seller.Web.Areas.Orders.ApiResponseModels;
using Seller.Web.Areas.Orders.Definitions;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.Baskets;
using Seller.Web.Areas.Orders.Services.OrderFiles;
using Seller.Web.Areas.Shared.Repositories.Products;
using Seller.Web.Shared.Configurations;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class OrderFileApiController : BaseApiController
    {
        private readonly IOrderFileService orderFileService;
        private readonly IProductsRepository productsRepository;
        private readonly IBasketRepository basketRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaHelperService mediaService;
        private readonly ILogger<OrderFileApiController> logger;

        public OrderFileApiController(
            IOrderFileService orderFileService,
            IProductsRepository productsRepository,
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator,
            IOptions<AppSettings> options,
            IMediaHelperService mediaService,
            ILogger<OrderFileApiController> logger)
        {
            this.orderFileService = orderFileService;
            this.productsRepository = productsRepository;
            this.basketRepository = basketRepository;
            this.linkGenerator = linkGenerator;
            this.options = options;
            this.mediaService = mediaService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] UploadMediaRequestModel model)
        {
            var importedOrderLines = this.orderFileService.ImportOrderLines(model.File);

            var basketItems = new List<BasketItem>();

            foreach (var orderLine in importedOrderLines)
            {
                var product = await this.productsRepository.GetProductAsync(
                    await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                    CultureInfo.CurrentUICulture.Name,
                    orderLine.Sku);

                if (product == null)
                {
                    this.logger.LogError($"Product for SKU {orderLine.Sku} and language {CultureInfo.CurrentUICulture.Name} couldn't be found.");
                }
                else
                {
                    var basketItem = new BasketItem
                    {
                        ProductId = product.Id,
                        ProductSku = product.Sku,
                        ProductName = product.Name,
                        PictureUrl = product.Images.OrEmptyIfNull().Any() ? this.mediaService.GetFileUrl(this.options.Value.MediaUrl, product.Images.First(), OrdersConstants.Basket.BasketProductImageMaxWidth, OrdersConstants.Basket.BasketProductImageMaxHeight, true) : null,
                        Quantity = orderLine.Quantity,
                        ExternalReference = orderLine.ExternalReference,
                        DeliveryFrom = orderLine.DeliveryFrom,
                        DeliveryTo = orderLine.DeliveryTo,
                        MoreInfo = orderLine.MoreInfo
                    };

                    basketItems.Add(basketItem);
                }
            }

            var basket = await this.basketRepository.SaveAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                model.Id,
                basketItems);

            var basketResponseModel = new BasketResponseModel
            {
                Id = basket.Id
            };

            var productIds = basket.Items.OrEmptyIfNull().Select(x => x.ProductId.Value);

            if (productIds.OrEmptyIfNull().Any())
            {
                basketResponseModel.Items = basket.Items.OrEmptyIfNull().Select(x => new BasketItemResponseModel
                {
                    ProductId = x.ProductId,
                    ProductUrl = this.linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                    Name = x.ProductName,
                    Sku = x.ProductSku,
                    Quantity = x.Quantity,
                    ExternalReference = x.ExternalReference,
                    ImageSrc = x.PictureUrl,
                    ImageAlt = x.ProductName,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                });
            }

            return this.StatusCode((int)HttpStatusCode.OK, basketResponseModel);
        }
    }
}