using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.Repositories.Baskets;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class BasketCheckoutApiController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;

        public BasketCheckoutApiController(
            IBasketRepository basketRepository,
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            _basketRepository = basketRepository;
            _orderLocalizer = orderLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutBasketRequestModel model)
        {
            await _basketRepository.CheckoutBasketAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                model.ClientId,
                model.ClientName,
                model.BasketId,
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
                model.ExpectedDeliveryDate,
                model.MoreInfo);

            return StatusCode((int)HttpStatusCode.Accepted, new { Message = _orderLocalizer.GetString("OrderPlacedSuccessfully").Value });
        }
    }
}
