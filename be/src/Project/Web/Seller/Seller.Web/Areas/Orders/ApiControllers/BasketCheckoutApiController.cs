using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
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

        public BasketCheckoutApiController(
            IBasketRepository basketRepository,
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IUserApprovalsRepository userApprovalsRepository,
            IClientsRepository clientsRepository,
            IIdentityRepository identityRepository,
            IBasketService basketService,
            IInventoryRepository inventoryRepository,
            IOutletRepository outletRepository)
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
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutBasketRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var basket = await _basketRepository.GetBasketByIdAsync(token, language, model.BasketId);

            if (basket is null || basket.Items.Any() is false)
            {
                throw new CustomException(_orderLocalizer.GetString("BasketNotFound").Value, (int)HttpStatusCode.NotFound);
            }

            var items = basket.Items.OrEmptyIfNull().ToList();

            if (items.Any(x => x.StockQuantity > 0 || x.OutletQuantity > 0))
            {
                if (items.Any(x => x.ProductId.HasValue is false))
                {
                    throw new CustomException(_orderLocalizer.GetString("ProductsNotFound"), (int)HttpStatusCode.NotFound);
                }

                var inventories = await _inventoryRepository.GetInventoryProductByProductIdsAsync(
                    token,
                    language,
                    items.Where(x => x.StockQuantity > 0).Select(x => x.ProductId.Value));

                var outlets = await _outletRepository.GetOutletProductsByProductsIdAsync(
                    token,
                    language,
                    items.Where(x => x.OutletQuantity > 0).Select(x => x.ProductId.Value));

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
                client.OrganisationId);

            return StatusCode((int)HttpStatusCode.Accepted, new { Message = _orderLocalizer.GetString("OrderPlacedSuccessfully").Value });
        }
    }
}
