using Buyer.Web.Areas.Orders.Repositories;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class OrderStatusApiController : BaseApiController
    {
        private readonly IOrdersRepository ordersRepository;

        public OrderStatusApiController(
            IOrdersRepository ordersRepository)
        {
            this.ordersRepository = ordersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var orders = await this.ordersRepository.GetOrderStatusesAsync(token, language);

            return this.StatusCode((int)HttpStatusCode.OK, orders);
        }
    }
}
