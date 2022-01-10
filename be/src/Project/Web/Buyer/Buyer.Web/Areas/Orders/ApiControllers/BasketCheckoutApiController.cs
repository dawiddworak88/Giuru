using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class BasketCheckoutApiController : BaseApiController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;

        public BasketCheckoutApiController(
            IBasketRepository basketRepository,
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            this.basketRepository = basketRepository;
            this.orderLocalizer = orderLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutBasketRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;
            await this.basketRepository.CheckoutBasketAsync(
                token, language, model.ClientId, model.ClientName, model.BasketId, model.ExpectedDeliveryDate, model.MoreInfo);

            return this.StatusCode((int)HttpStatusCode.Accepted, new { Message = this.orderLocalizer.GetString("OrderPlacedSuccessfully").Value });
        }
    }
}
