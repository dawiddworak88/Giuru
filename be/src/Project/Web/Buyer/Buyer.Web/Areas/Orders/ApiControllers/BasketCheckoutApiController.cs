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
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class BasketCheckoutApiController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IClientDeliveryAddressesRepository _clientDeliveryAddressesRepository;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;

        public BasketCheckoutApiController(
            IBasketRepository basketRepository,
            IClientDeliveryAddressesRepository clientDeliveryAddressesRepository,
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            _basketRepository = basketRepository;
            _orderLocalizer = orderLocalizer;
            _clientDeliveryAddressesRepository = clientDeliveryAddressesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutBasketRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var reqCookie = Request.Cookies[BasketConstants.BasketCookieName];

            if (reqCookie is null)
            {
                reqCookie = Guid.NewGuid().ToString();

                var cookieOptions = new CookieOptions
                {
                    MaxAge = TimeSpan.FromDays(BasketConstants.BasketCookieMaxAge)
                };

                Response.Cookies.Append(BasketConstants.BasketCookieName, reqCookie, cookieOptions);
            }

            var deliveryAddress = await _clientDeliveryAddressesRepository.GetAsync(token, language, model.ShippingAddressId);

            if (deliveryAddress is not null)
            {
                await _basketRepository.CheckoutBasketAsync(
                token,
                language,
                model.ClientId,
                model.ClientName,
                Guid.Parse(reqCookie),
                model.ShippingAddressId,
                deliveryAddress.Company,
                deliveryAddress.FirstName,
                deliveryAddress.LastName,
                deliveryAddress.Region,
                deliveryAddress.PostCode,
                deliveryAddress.City,
                deliveryAddress.Street,
                deliveryAddress.PhoneNumber,
                deliveryAddress.CountryId,
                model.ExpectedDeliveryDate,
                model.MoreInfo,
                model.HasCustomOrder,
                model.Attachments?.Select(x => x.Id));

                return StatusCode((int)HttpStatusCode.Accepted, new { Message = _orderLocalizer.GetString("OrderPlacedSuccessfully").Value });
            }

            return StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
