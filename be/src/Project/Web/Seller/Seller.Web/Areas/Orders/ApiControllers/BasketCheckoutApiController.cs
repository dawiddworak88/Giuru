using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.Baskets;
using Seller.Web.Areas.Orders.Repositories.OrderAttributeValues;
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
        private readonly IOrderAttributeValuesRepository _orderAttributeValuesRepository;

        public BasketCheckoutApiController(
            IBasketRepository basketRepository,
            IStringLocalizer<OrderResources> orderLocalizer,
            IOrderAttributeValuesRepository orderAttributeValuesRepository)
        {
            _basketRepository = basketRepository;
            _orderLocalizer = orderLocalizer;
            _orderAttributeValuesRepository = orderAttributeValuesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutBasketRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _basketRepository.CheckoutBasketAsync(
                token,
                language,
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
                model.MoreInfo);

            if (model.AttributesValues is not null && model.AttributesValues.Any())
            {
                await _orderAttributeValuesRepository.BatchAsync(token, language, model.BasketId, 
                    model.AttributesValues.Select(x => new ApiOrderAttributeValue
                    {
                        AttributeId = x.AttributeId,
                        Value = x.Value
                    }));
            }

            return StatusCode((int)HttpStatusCode.Accepted, new { Message = _orderLocalizer.GetString("OrderPlacedSuccessfully").Value });
        }
    }
}
