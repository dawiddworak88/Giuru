﻿using Buyer.Web.Areas.Orders.DomainModels;
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
        private readonly IOrdersRepository ordersRepository;

        public OrdersApiController(
            IOrdersRepository ordersRepository)
        {
            this.ordersRepository = ordersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var orders = await this.ordersRepository.GetOrdersAsync(token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(Order.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, orders);
        }
    }
}
