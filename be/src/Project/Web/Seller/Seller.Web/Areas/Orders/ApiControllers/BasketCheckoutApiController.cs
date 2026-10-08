using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Foundation.Pricing.Configurations;
using Foundation.Pricing.DiscountCodes;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.Services.DiscountCodes;
using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Areas.Inventory.Repositories;
using Seller.Web.Areas.Inventory.Repositories.Inventories;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.Definitions;
using Seller.Web.Areas.Orders.Repositories.Baskets;
using Seller.Web.Areas.Orders.Services.Basket;
using Seller.Web.Areas.Shared.Repositories.UserApprovals;
using Seller.Web.Shared.DomainModels.UserApproval;
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Shared.Repositories.Identity;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class BasketCheckoutApiController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IUserApprovalsRepository _userApprovalsRepository;
        private readonly IClientsRepository _clientsRepository;
        private readonly IIdentityRepository _identityRepository;
        private readonly IBasketService _basketService;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IOutletRepository _outletRepository;
        private readonly IOptions<AppSettings> _options;
        private readonly IDiscountCodeValidator _discountCodeValidator;

        public BasketCheckoutApiController(
            IBasketRepository basketRepository,
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IUserApprovalsRepository userApprovalsRepository,
            IClientsRepository clientsRepository,
            IIdentityRepository identityRepository,
            IBasketService basketService,
            IInventoryRepository inventoryRepository,
            IOutletRepository outletRepository,
            IOptions<AppSettings> options,
            IDiscountCodeValidator discountCodeValidator)
        {
            _basketRepository = basketRepository;
            _orderLocalizer = orderLocalizer;
            _clientLocalizer = clientLocalizer;
            _userApprovalsRepository = userApprovalsRepository;
            _clientsRepository = clientsRepository;
            _identityRepository = identityRepository;
            _basketService = basketService;
            _inventoryRepository = inventoryRepository;
            _outletRepository = outletRepository;
            _options = options;
            _discountCodeValidator = discountCodeValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutBasketRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var basket = await _basketRepository.GetBasketByIdAsync(token, language, model.BasketId);

            if (basket is null || basket.Items.OrEmptyIfNull().Any() is false)
            {
                throw new CustomException(_orderLocalizer.GetString("BasketNotFound").Value, (int)HttpStatusCode.NotFound);
            }

            var items = basket.Items.ToList();

            // Enforcement at checkout. The order is placed for model.ClientId, so that is the client the stored code must
            // be applicable for, and the stored basket must be the snapshot that was validated for that same client.
            Guid? expectedBasketVersion = null;

            if (_options.Value.IsDiscountCodeEnforced())
            {
                // A basket without a version predates the snapshot guard and must be saved once. Even a basket with no
                // code needs one: a concurrent save could add a code between this read and the checkout.
                if (basket.BasketVersion.HasValue is false)
                {
                    return Conflict("DiscountCodeBasketNeedsRefresh");
                }

                expectedBasketVersion = basket.BasketVersion;

                if (string.IsNullOrWhiteSpace(basket.DiscountCode) is false)
                {
                    var validation = await _discountCodeValidator.ValidateAsync(model.ClientId, basket.DiscountCode, token, HttpContext.RequestAborted);

                    if (validation.Status is DiscountCodeValidationStatus.Unavailable)
                    {
                        return StatusCode((int)HttpStatusCode.ServiceUnavailable, new { Message = _orderLocalizer.GetString("DiscountCodeCouldNotBeVerified").Value });
                    }

                    if (validation.IsValid is false)
                    {
                        return StatusCode((int)HttpStatusCode.Conflict, new { Message = _orderLocalizer.GetString("DiscountCodeNoLongerValid", basket.DiscountCode).Value });
                    }

                    // Applicable for this client now, but the stored prices must also have been calculated for it. A code
                    // assigned to both client A and client B would otherwise carry A's prices into B's order.
                    if (basket.DiscountCodeClientId != model.ClientId
                        || string.Equals(validation.DiscountCode, basket.DiscountCode, StringComparison.Ordinal) is false)
                    {
                        return Conflict("DiscountCodeBasketNeedsRefresh");
                    }
                }
            }

            if (items.Any(x => x.StockQuantity > 0 || x.OutletQuantity > 0))
            {
                if (items.Any(x => (x.StockQuantity > 0 || x.OutletQuantity > 0) && x.ProductId.HasValue is false))
                {
                    throw new CustomException(_orderLocalizer.GetString("ProductsNotFound").Value, (int)HttpStatusCode.NotFound);
                }

                var inventoriesIds = items.Where(x => x.StockQuantity > 0).Select(x => x.ProductId.Value).ToList();
                var outletsIds = items.Where(x => x.OutletQuantity > 0).Select(x => x.ProductId.Value).ToList();

                var inventoriesTask = inventoriesIds.Any()
                    ? _inventoryRepository.GetInventoryProductByProductIdsAsync(token, language, inventoriesIds)
                    : Task.FromResult(Enumerable.Empty<InventoryItem>());

                var outletsTask = outletsIds.Any()
                    ? _outletRepository.GetOutletProductsByProductsIdAsync(token, language, outletsIds)
                    : Task.FromResult(Enumerable.Empty<OutletItem>());

                await Task.WhenAll(inventoriesTask, outletsTask);

                var inventories = inventoriesTask.Result;
                var outlets = outletsTask.Result;

                _basketService.ValidateStockOutletQuantities(items, inventories, outlets);
            }

            var userApprovals = Enumerable.Empty<UserApproval>();

            var client = await _clientsRepository.GetClientAsync(token, language, model.ClientId);

            if (client is null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new { Message = _clientLocalizer.GetString("ClientNotFound").Value });
            }

            var user = await _identityRepository.GetAsync(token, language, client.Email);

            if (user is not null)
            {
                userApprovals = await _userApprovalsRepository.GetAsync(token, language, Guid.Parse(user.Id));
            }

            await _basketRepository.CheckoutBasketAsync(
                token,
                language,
                model.ClientId,
                model.ClientName,
                client.Email,
                model.BasketId,
                model.BillingAddressId,
                model.BillingCompany,
                model.BillingFirstName,
                model.BillingLastName,
                model.BillingRegion,
                model.BillingPostCode,
                model.BillingCity,
                model.BillingStreet,
                model.BillingPhoneNumber,
                model.BillingCountryId,
                model.ShippingAddressId,
                model.ShippingCompany,
                model.ShippingFirstName,
                model.ShippingLastName,
                model.ShippingRegion,
                model.ShippingPostCode,
                model.ShippingCity,
                model.ShippingStreet,
                model.ShippingPhoneNumber,
                model.ShippingCountryId,
                model.MoreInfo,
                userApprovals.Any(x => x.ApprovalId == ApprovalsConstants.SendOrderConfirmationEmailId),
                client.OrganisationId,
                expectedBasketVersion);

            return StatusCode((int)HttpStatusCode.Accepted, new { Message = _orderLocalizer.GetString("OrderPlacedSuccessfully").Value });
        }

        private IActionResult Conflict(string resourceKey)
        {
            return StatusCode((int)HttpStatusCode.Conflict, new { Message = _orderLocalizer.GetString(resourceKey).Value });
        }
    }
}
