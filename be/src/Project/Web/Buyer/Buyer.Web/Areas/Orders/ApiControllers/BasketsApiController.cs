﻿using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Orders.Definitions;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Basket;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Services.MediaServices;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    [Authorize]
    public class BasketsApiController : BaseApiController
    {
        private readonly IBasketRepository basketRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaHelperService mediaService;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;

        public BasketsApiController(
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator,
            IOptions<AppSettings> options,
            IMediaHelperService mediaService,
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            this.basketRepository = basketRepository;
            this.linkGenerator = linkGenerator;
            this.options = options;
            this.mediaService = mediaService;
            this.orderLocalizer = orderLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveBasketRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var reqCookie = this.Request.Cookies[BasketConstants.BasketCookieName];
            if (reqCookie is null)
            {
                reqCookie = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions
                {
                    MaxAge = TimeSpan.FromDays(BasketConstants.BasketCookieMaxAge)
                };
                this.Response.Cookies.Append(BasketConstants.BasketCookieName, reqCookie, cookieOptions);
            }

            var id = Guid.Parse(reqCookie);
            var basket = await this.basketRepository.SaveAsync(token, language, id,
                model.Items.OrEmptyIfNull().Select(x => new BasketItem
                {
                    ProductId = x.ProductId,
                    ProductSku = x.Sku,
                    ProductName = x.Name,
                    PictureUrl = !string.IsNullOrWhiteSpace(x.ImageSrc) ? x.ImageSrc : (x.ImageId.HasValue ? this.mediaService.GetFileUrl(this.options.Value.MediaUrl, x.ImageId.Value, OrdersConstants.Basket.BasketProductImageMaxWidth, OrdersConstants.Basket.BasketProductImageMaxHeight, true) : null),
                    Quantity = x.Quantity,
                    StockQuantity = x.StockQuantity,
                    OutletQuantity = x.OutletQuantity,
                    ExternalReference = x.ExternalReference,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                }));

            //Console.WriteLine(JsonConvert.SerializeObject(basket.Items));

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
                    ProductUrl = this.linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = language, Id = x.ProductId }),
                    Name = x.ProductName,
                    Sku = x.ProductSku,
                    Quantity = x.Quantity,
                    StockQuantity = x.StockQuantity,
                    OutletQuantity = x.OutletQuantity,
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

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.basketRepository.DeleteAsync(token, language, id);

            this.Response.Cookies.Delete(BasketConstants.BasketCookieName);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.orderLocalizer.GetString("BasketDeletedSuccessfully").Value });
        }
    }
}
