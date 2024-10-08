using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.Definitions;
using Buyer.Web.Areas.Orders.Repositories;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class OrderStatusApiController : BaseApiController
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;

        public OrderStatusApiController(
            IOrdersRepository ordersRepository,
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            _ordersRepository = ordersRepository;
            _orderLocalizer = orderLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Cancel([FromBody] UpdateOrderStatusRequestModel model)
        {
            if (model.OrderStatusId.Equals(OrdersConstants.OrderStatuses.NewId) is false)
            {
                throw new CustomException(_orderLocalizer.GetString("CancellationOrderError"), (int)HttpStatusCode.BadRequest);
            }

            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var orderStatusId = await _ordersRepository.SaveOrderStatusAsync(token, language, model.OrderId, OrdersConstants.OrderStatuses.CancelId);

            return StatusCode((int)HttpStatusCode.OK, new { OrderStatusId = orderStatusId, Message = _orderLocalizer.GetString("SuccessfullyCanceledOrder").Value });
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrderItem([FromBody] UpdateOrderItemStatusRequestModel request)
        {
            if (request.OrderItemStatusId.Equals(OrdersConstants.OrderStatuses.NewId) is false)
            {
                throw new CustomException(_orderLocalizer.GetString("CancellationOrderError"), (int)HttpStatusCode.BadRequest);
            }

            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _ordersRepository.UpdateOrderItemStatusAsync(token, language, request.Id, OrdersConstants.OrderStatuses.CancelId);

            var orderItemStatusChanges = await _ordersRepository.GetOrderItemStatusesAsync(token, language, request.Id);

            return StatusCode((int)HttpStatusCode.OK, new { StatusChanges = orderItemStatusChanges.StatusChanges, Message = _orderLocalizer.GetString("SuccessfullyCanceledOrder").Value });
        }
    }
}
