using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.Definitions;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Orders.Repositories.UserApprovals;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Basket;
using Buyer.Web.Shared.Extensions;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Identity;
using Buyer.Web.Shared.Services.Baskets;
using Buyer.Web.Shared.Services.DiscountCodes;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Foundation.Pricing.DiscountCodes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
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
        private readonly IDiscountCodeValidator _discountCodeValidator;
        private readonly IOptions<AppSettings> _options;
        private readonly ILogger<BasketCheckoutApiController> _logger;

        public BasketCheckoutApiController(
            IBasketRepository basketRepository,
            IBasketService basketService,
            IClientAddressesRepository clientAddressesRepository,
            IStringLocalizer<OrderResources> orderLocalizer,
            IUserApprovalsRepository userApprovalsRepository,
            IIdentityRepository identityRepository,
            IDiscountCodeValidator discountCodeValidator,
            IOptions<AppSettings> options,
            ILogger<BasketCheckoutApiController> logger)
        {
            _basketRepository = basketRepository;
            _basketService = basketService;
            _orderLocalizer = orderLocalizer;
            _clientAddressesRepository = clientAddressesRepository;
            _userApprovalsRepository = userApprovalsRepository;
            _identityRepository = identityRepository;
            _discountCodeValidator = discountCodeValidator;
            _options = options;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutBasketRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;
            var organisationId = GuidHelper.ParseNullable(User.FindFirstValue(AccountConstants.Claims.OrganisationIdClaim));
            var clientId = User.GetClientId();

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

            var basketId = Guid.Parse(reqCookie);
            var basket = await _basketRepository.GetBasketById(token, language, basketId);

            // Basket.Api returns an empty basket when the ID is new; null means the read failed, so fail closed.
            if (basket is null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new { Message = _orderLocalizer.GetString("BasketPricesCouldNotBeVerified").Value });
            }

            // The order keeps the prices the basket was saved with, so the stored code has to be applicable for the buyer's
            // client now too: a code revoked since the last save, or a basket carried over from another client, is refused
            // until the buyer removes it and sees the prices without it. A code that cannot be verified right now was
            // applicable when the basket was priced with it, so an outage of the lookup does not block the order.
            if (_options.Value.IsGrulaConfigured && !string.IsNullOrWhiteSpace(basket.DiscountCode))
            {
                var validation = await _discountCodeValidator.ValidateAsync(clientId, basket.DiscountCode, token);

                if (validation.Status is DiscountCodeValidationStatus.Unavailable)
                {
                    _logger.LogWarning("The discount code stored on basket {BasketId} could not be verified at checkout. The order keeps the prices the basket was saved with.", basketId);
                }
                else if (!validation.IsValid)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { Message = DiscountCodeMessages.GetCheckoutRejectionMessage(_orderLocalizer, basket.DiscountCode) });
                }
            }

            await _basketService.ValidateStockOutletQuantitiesAsync(basketId, token, language);

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
                organisationId);

            return StatusCode((int)HttpStatusCode.Accepted, new { Message = _orderLocalizer.GetString("OrderPlacedSuccessfully").Value });
        }
    }
}
