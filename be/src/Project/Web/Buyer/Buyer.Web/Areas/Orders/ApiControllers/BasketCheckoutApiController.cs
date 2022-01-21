using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Shared.Repositories.Clients;
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
        private readonly IClientsRepository clientsRepository;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;

        public BasketCheckoutApiController(
            IBasketRepository basketRepository,
            IClientsRepository clientsRepository,
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            this.basketRepository = basketRepository;
            this.orderLocalizer = orderLocalizer;
            this.clientsRepository = clientsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutBasketRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var client = await this.clientsRepository.GetClientAsync(token, language);

            await this.basketRepository.CheckoutBasketAsync(
                token, language, client.Id, client.Name, model.BasketId, model.ExpectedDeliveryDate, model.MoreInfo);

            return this.StatusCode((int)HttpStatusCode.Accepted, new { Message = this.orderLocalizer.GetString("OrderPlacedSuccessfully").Value });
        }
    }
}
