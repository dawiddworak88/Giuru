using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.Definitions;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Orders.Repositories.NotificationTypeApproval;
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
using System.Collections.Generic;
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
        private readonly IClientAddressesRepository _clientAddressesRepository;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IClientNotificationTypeApproval _clientNotificationTypeRepository;

        public BasketCheckoutApiController(
            IBasketRepository basketRepository,
            IClientAddressesRepository clientAddressesRepository,
            IStringLocalizer<OrderResources> orderLocalizer,
            IClientNotificationTypeApproval clientNotificationTypeRepository)
        {
            _basketRepository = basketRepository;
            _orderLocalizer = orderLocalizer;
            _clientAddressesRepository = clientAddressesRepository;
            _clientNotificationTypeRepository = clientNotificationTypeRepository; 
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

            var deliveryAddressesIds = new List<Guid>();

            if (model.ShippingAddressId.HasValue)
            {
                deliveryAddressesIds.Add(model.ShippingAddressId.Value);
            }

            if (model.BillingAddressId.HasValue && model.BillingAddressId != model.ShippingAddressId)
            {
                deliveryAddressesIds.Add(model.BillingAddressId.Value);
            }

            var clientApprovlas = await _clientNotificationTypeRepository.GetAsync(token, language, model.ClientId);

            var clientAddresses = await _clientAddressesRepository.GetAsync(token, language, deliveryAddressesIds);

            await _basketRepository.CheckoutBasketAsync(
                token,
                language,
                model.ClientId,
                model.ClientName,
                Guid.Parse(reqCookie),
                clientAddresses?.FirstOrDefault(x => x.Id == model.BillingAddressId),
                clientAddresses?.FirstOrDefault(x => x.Id == model.ShippingAddressId),
                model.MoreInfo,
                model.HasCustomOrder,
                clientApprovlas.Any(x => x.NotificationTypeId == ClientNotificationTypeConstants.ApprovalToSendOrderConfirmationEmailsId),
                model.Attachments?.Select(x => x.Id));

            return StatusCode((int)HttpStatusCode.Accepted, new { Message = _orderLocalizer.GetString("OrderPlacedSuccessfully").Value });
        }
    }
}
