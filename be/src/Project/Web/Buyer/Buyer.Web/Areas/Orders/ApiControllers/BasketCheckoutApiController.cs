using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.Definitions;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Orders.Repositories.UserApprovals;
using Buyer.Web.Shared.Definitions.Basket;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Identity;
using Buyer.Web.Shared.Services.Baskets;
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
using System.Security.Claims;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class BasketCheckoutApiController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IBasketService _basketService;
        private readonly IClientAddressesRepository _clientAddressesRepository;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IUserApprovalsRepository _userApprovalsRepository;
        private readonly IIdentityRepository _identityRepository;

        public BasketCheckoutApiController(
            IBasketRepository basketRepository,
            IBasketService basketService,
            IClientAddressesRepository clientAddressesRepository,
            IStringLocalizer<OrderResources> orderLocalizer,
            IUserApprovalsRepository userApprovalsRepository,
            IIdentityRepository identityRepository)
        {
            _basketRepository = basketRepository;
            _basketService = basketService;
            _orderLocalizer = orderLocalizer;
            _clientAddressesRepository = clientAddressesRepository;
            _userApprovalsRepository = userApprovalsRepository;
            _identityRepository = identityRepository;
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

            await _basketService.ValidateStockOutletQuantitiesAsync(model.BasketId, token, language);

            var deliveryAddressesIds = new List<Guid>();

            if (model.ShippingAddressId.HasValue)
            {
                deliveryAddressesIds.Add(model.ShippingAddressId.Value);
            }

            if (model.BillingAddressId.HasValue && model.BillingAddressId != model.ShippingAddressId)
            {
                deliveryAddressesIds.Add(model.BillingAddressId.Value);
            }

            var user = await _identityRepository.GetAsync(token, language, User.FindFirstValue(ClaimTypes.Email));

            var clientApprovlas = Enumerable.Empty<UserApproval>();

            if (user is not null)
            {
                clientApprovlas = await _userApprovalsRepository.GetAsync(token, language, Guid.Parse(user.Id));
            }

            var clientAddresses = await _clientAddressesRepository.GetAsync(token, language, deliveryAddressesIds);

            await _basketRepository.CheckoutBasketAsync(
                token,
                language,
                model.ClientId,
                model.ClientName,
                User.FindFirstValue(ClaimTypes.Email),
                Guid.Parse(reqCookie),
                clientAddresses?.FirstOrDefault(x => x.Id == model.BillingAddressId),
                clientAddresses?.FirstOrDefault(x => x.Id == model.ShippingAddressId),
                model.MoreInfo,
                model.HasCustomOrder,
                clientApprovlas.Any(x => x.ApprovalId == ApprovalsConstants.ToSendOrderConfirmationEmails),
                model.Attachments?.Select(x => x.Id));

            return StatusCode((int)HttpStatusCode.Accepted, new { Message = _orderLocalizer.GetString("OrderPlacedSuccessfully").Value });
        }
    }
}
