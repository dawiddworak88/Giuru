using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.Definitions;
using Seller.Web.Areas.Orders.Repositories.Baskets;
using Seller.Web.Areas.Orders.Repositories.ClientNotificationTypeApproval;
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
        private readonly IClientNotificationTypeApprovalRepository _notificationTypeApprovalRepository;

        public BasketCheckoutApiController(
            IBasketRepository basketRepository,
            IStringLocalizer<OrderResources> orderLocalizer,
            IClientNotificationTypeApprovalRepository notificationTypeApprovalRepository)
        {
            _basketRepository = basketRepository;
            _orderLocalizer = orderLocalizer;
            _notificationTypeApprovalRepository = notificationTypeApprovalRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutBasketRequestModel model)
        {
            var clientApprovals = await _notificationTypeApprovalRepository.GetAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName), 
                CultureInfo.CurrentUICulture.Name, model.ClientId);

            await _basketRepository.CheckoutBasketAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                model.ClientId,
                model.ClientName,
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
                clientApprovals.Any(x => x.NotificationTypeId == ClientNotificationTypeConstants.ApprovalToSendOrderConfirmationEmailsId));

            return StatusCode((int)HttpStatusCode.Accepted, new { Message = _orderLocalizer.GetString("OrderPlacedSuccessfully").Value });
        }
    }
}
