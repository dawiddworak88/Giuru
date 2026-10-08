using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.Definitions;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Orders.Repositories.UserApprovals;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Basket;
using Buyer.Web.Shared.Extensions;
using Buyer.Web.Shared.Services.DiscountCodes;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Identity;
using Buyer.Web.Shared.Services.Baskets;
using Foundation.Account.Definitions;
using Foundation.Pricing.Configurations;
using Foundation.Pricing.DiscountCodes;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
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
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IOptions<AppSettings> _options;
        private readonly IDiscountCodeValidator _discountCodeValidator;

        public BasketCheckoutApiController(
            IBasketRepository basketRepository,
            IBasketService basketService,
            IClientAddressesRepository clientAddressesRepository,
            IStringLocalizer<OrderResources> orderLocalizer,
            IUserApprovalsRepository userApprovalsRepository,
            IIdentityRepository identityRepository,
            IStringLocalizer<ClientResources> clientLocalizer,
            IOptions<AppSettings> options,
            IDiscountCodeValidator discountCodeValidator)
        {
            _basketRepository = basketRepository;
            _basketService = basketService;
            _orderLocalizer = orderLocalizer;
            _clientAddressesRepository = clientAddressesRepository;
            _userApprovalsRepository = userApprovalsRepository;
            _identityRepository = identityRepository;
            _clientLocalizer = clientLocalizer;
            _options = options;
            _discountCodeValidator = discountCodeValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutBasketRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;
            var organisationId = GuidHelper.ParseNullable(User.FindFirstValue(AccountConstants.Claims.OrganisationIdClaim));

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

            // With enforcement on, the identities that the discount code checks are made for must be the ones the order is
            // placed for: the client comes from the authenticated principal and the basket from the cookie, never from the
            // request body. With it off nothing changes.
            var isDiscountCodeEnforced = _options.Value.IsDiscountCodeEnforced();
            var basketId = Guid.Parse(reqCookie);
            var clientId = model.ClientId;
            Guid? expectedBasketVersion = null;

            if (isDiscountCodeEnforced)
            {
                // Every discount code check below is made for the principal's client, whatever the request says. A principal
                // without a client of its own - a client team member, whose client the order form resolves by organisation
                // rather than by email - keeps the client of the request for the order, as before enforcement: rejecting it
                // would stop those users from ordering at all. No code can be verified for such a principal, so a basket
                // that holds one is refused below and the order never carries a discount.
                var principalClientId = User.GetClientId();

                if (principalClientId.HasValue)
                {
                    if (model.ClientId.HasValue && model.ClientId != principalClientId)
                    {
                        return StatusCode((int)HttpStatusCode.BadRequest, new { Message = _clientLocalizer.GetString("ClientNotFound").Value });
                    }

                    clientId = principalClientId;
                }

                var basket = await _basketRepository.GetBasketById(token, language, basketId);
                var hasBasketContent = basket is not null
                    && (basket.Items.OrEmptyIfNull().Any() || string.IsNullOrWhiteSpace(basket.DiscountCode) is false);

                if (hasBasketContent is false)
                {
                    // Nothing is stored (or nothing worth guarding): the custom order path. Assert that it stays that way.
                    expectedBasketVersion = Guid.Empty;
                }
                else if (basket.BasketVersion.HasValue is false)
                {
                    // Saved before the snapshot guard existed: it must be saved once to acquire a version.
                    return StatusCode((int)HttpStatusCode.Conflict, new { Message = _orderLocalizer.GetString("DiscountCodeBasketNeedsRefresh").Value });
                }
                else
                {
                    // Even a basket without a code needs its version: a concurrent save could add a code before the order.
                    expectedBasketVersion = basket.BasketVersion;
                }

                if (string.IsNullOrWhiteSpace(basket?.DiscountCode) is false)
                {
                    var validation = await _discountCodeValidator.ValidateAsync(principalClientId, basket.DiscountCode, token, HttpContext.RequestAborted);

                    if (validation.Status is DiscountCodeValidationStatus.Unavailable)
                    {
                        return StatusCode((int)HttpStatusCode.ServiceUnavailable, new { Message = _orderLocalizer.GetString("DiscountCodeCouldNotBeVerified").Value });
                    }

                    if (validation.IsValid is false)
                    {
                        return StatusCode((int)HttpStatusCode.Conflict, new { Message = _orderLocalizer.GetString("DiscountCodeNoLongerValid", basket.DiscountCode).Value });
                    }

                    // Applicable for this client now, but the stored prices must also have been calculated for it.
                    if (basket.DiscountCodeClientId != principalClientId
                        || string.Equals(validation.DiscountCode, basket.DiscountCode, StringComparison.Ordinal) is false)
                    {
                        return StatusCode((int)HttpStatusCode.Conflict, new { Message = _orderLocalizer.GetString("DiscountCodeBasketNeedsRefresh").Value });
                    }
                }
            }

            await _basketService.ValidateStockOutletQuantitiesAsync(isDiscountCodeEnforced ? basketId : model.BasketId, token, language);

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
                clientId,
                model.ClientName,
                User.FindFirstValue(ClaimTypes.Email),
                basketId,
                clientAddresses?.FirstOrDefault(x => x.Id == model.BillingAddressId),
                clientAddresses?.FirstOrDefault(x => x.Id == model.ShippingAddressId),
                model.MoreInfo,
                model.HasCustomOrder,
                clientApprovlas.Any(x => x.ApprovalId == ApprovalsConstants.ToSendOrderConfirmationEmails),
                model.Attachments?.Select(x => x.Id),
                organisationId,
                expectedBasketVersion);

            return StatusCode((int)HttpStatusCode.Accepted, new { Message = _orderLocalizer.GetString("OrderPlacedSuccessfully").Value });
        }
    }
}
