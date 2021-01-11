using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.Baskets;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class BasketsApiController : BaseApiController
    {
        private readonly IBasketsRepository basketsRepository;

        public BasketsApiController(
            IBasketsRepository basketsRepository)
        {
            this.basketsRepository = basketsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveBasketRequestModel model)
        {
            var basket = await this.basketsRepository.SaveAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                model.Id,
                model.Items.OrEmptyIfNull().Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                }));

            return this.StatusCode((int)HttpStatusCode.OK, basket);
        }
    }
}
