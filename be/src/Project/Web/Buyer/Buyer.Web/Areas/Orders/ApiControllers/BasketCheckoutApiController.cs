using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Shared.Definitions.Basket;
using Buyer.Web.Shared.Repositories.Clients;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class BasketCheckoutApiController : BaseApiController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IClientsRepository clientsRepository;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;

        public BasketCheckoutApiController(
            IBasketRepository basketRepository,
            IClientsRepository clientsRepository,
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            this.basketRepository = basketRepository;
            this.orderLocalizer = orderLocalizer;
            this.clientsRepository = clientsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutBasketRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var client = await this.clientsRepository.GetClientAsync(token, language);

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

            await this.basketRepository.CheckoutBasketAsync(
                token, language, client.Id, client.Name, Guid.Parse(reqCookie), model.ExpectedDeliveryDate, model.MoreInfo);

            return this.StatusCode((int)HttpStatusCode.Accepted, new { Message = this.orderLocalizer.GetString("OrderPlacedSuccessfully").Value });
        }
    }
}
