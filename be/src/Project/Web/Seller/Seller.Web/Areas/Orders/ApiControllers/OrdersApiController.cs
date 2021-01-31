using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.Orders;
using Seller.Web.Shared.Repositories.Clients;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class OrdersApiController : BaseApiController
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly IClientsRepository clientsRepository;

        public OrdersApiController(
            IOrdersRepository ordersRepository,
            IClientsRepository clientsRepository)
        {
            this.ordersRepository = ordersRepository;
            this.clientsRepository = clientsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var orders = await this.ordersRepository.GetOrdersAsync(
                token,
                language,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(Order.CreatedDate)} desc");

            if (orders.Data.Any())
            {
                var clientIds = orders.Data.Select(x => x.ClientId).Distinct();

                var clients = await this.clientsRepository.GetClientsAsync(token, language, clientIds);

                foreach (var order in orders.Data)
                {
                    order.ClientName = clients.FirstOrDefault(x => x.Id == order.ClientId)?.Name;
                }
            }

            return this.StatusCode((int)HttpStatusCode.OK, orders);
        }
    }
}
