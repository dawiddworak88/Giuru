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
            await this.basketRepository.CheckoutBasketAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                model.ClientId,
                model.ClientName,
                model.BasketId,
                model.ExpectedDeliveryDate,
                model.MoreInfo);

            return this.StatusCode((int)HttpStatusCode.Accepted, new { Message = this.orderLocalizer.GetString("OrderPlacedSuccessfully").Value });
        }
    }
}
