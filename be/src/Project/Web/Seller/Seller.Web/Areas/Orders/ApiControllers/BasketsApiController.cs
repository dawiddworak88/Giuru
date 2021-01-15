using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Services.MediaServices;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.ApiResponseModels;
using Seller.Web.Areas.Orders.Definitions;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.Baskets;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Shared.Configurations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class BasketsApiController : BaseApiController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IProductsRepository productsRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaHelperService mediaService;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public BasketsApiController(
            IBasketRepository basketRepository,
            IProductsRepository productsRepository,
            LinkGenerator linkGenerator,
            IOptions<AppSettings> options,
            IMediaHelperService mediaService,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.basketRepository = basketRepository;
            this.productsRepository = productsRepository;
            this.linkGenerator = linkGenerator;
            this.options = options;
            this.mediaService = mediaService;
            this.globalLocalizer = globalLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveBasketRequestModel model)
        {
            var basket = await this.basketRepository.SaveAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                model.Id,
                model.Items.OrEmptyIfNull().Select(x => new BasketItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                }));

            var basketResponseModel = new BasketResponseModel
            {
                Id = basket.Id
            };

            var productIds = basket.Items.OrEmptyIfNull().Select(x => x.ProductId.Value);

            if (productIds.OrEmptyIfNull().Any())
            {
                var products = await this.productsRepository.GetAllProductsAsync(
                    await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                    CultureInfo.CurrentUICulture.Name,
                    productIds);

                if (products.OrEmptyIfNull().Any())
                {
                    basketResponseModel.Items = basket.Items.OrEmptyIfNull().Select(x => new BasketItemResponseModel
                    {
                        ProductId = x.ProductId,
                        ProductUrl = this.linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                        Name = products.First(p => p.Id == x.ProductId).Name,
                        Sku = products.First(p => p.Id == x.ProductId).Sku,
                        Quantity = x.Quantity,
                        ImageSrc = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, products.First(p => p.Id == x.ProductId).Images.FirstOrDefault(), OrdersConstants.Basket.BasketProductImageMaxWidth, OrdersConstants.Basket.BasketProductImageMaxHeight, true),
                        ImageAlt = products.First(p => p.Id == x.ProductId).Name,
                        DeliveryFrom = x.DeliveryFrom,
                        DeliveryTo = x.DeliveryTo,
                        MoreInfo = x.MoreInfo
                    });
                }                
            }

            return this.StatusCode((int)HttpStatusCode.OK, basketResponseModel);
        }
    }
}
