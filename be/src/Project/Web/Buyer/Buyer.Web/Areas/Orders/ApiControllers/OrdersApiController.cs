using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class OrdersApiController : BaseApiController
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersApiController(
            IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            string searchTerm, 
            int pageIndex, 
            int itemsPerPage, 
            Guid? orderStatusId)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var orders = await _ordersRepository.GetOrdersAsync(
                token, 
                language, 
                searchTerm, 
                pageIndex, 
                itemsPerPage, 
                $"{nameof(Order.CreatedDate)} desc",
                orderStatusId);

            return StatusCode((int)HttpStatusCode.OK, orders);
        }
    }
}
