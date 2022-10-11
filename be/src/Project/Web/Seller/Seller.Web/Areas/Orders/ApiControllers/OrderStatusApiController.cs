using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.Definitions;
using Seller.Web.Areas.Orders.Repositories.Orders;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class OrderStatusApiController : BaseApiController
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;

        public OrderStatusApiController(
            IOrdersRepository ordersRepository,
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            this.ordersRepository = ordersRepository;
            this.orderLocalizer = orderLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Cancel([FromBody] UpdateOrderStatusRequestModel model)
        {
            if (!model.OrderStatusId.Equals(OrdersConstants.OrderStatuses.NewId))
            {
                throw new CustomException(this.orderLocalizer.GetString("CancellationOrderError"), (int)HttpStatusCode.BadRequest);
            }

            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var orderStatusId = await this.ordersRepository.SaveOrderStatusAsync(token, language, model.OrderId, OrdersConstants.OrderStatuses.CancelId);

            return this.StatusCode((int)HttpStatusCode.OK, new { OrderStatusId = orderStatusId, Message = this.orderLocalizer.GetString("SuccessfullyCanceledOrder").Value });
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody]UpdateOrderStatusRequestModel model)
        {
            var orderStatusId = await this.ordersRepository.SaveOrderStatusAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                model.OrderId,
                model.OrderStatusId);

            return this.StatusCode((int)HttpStatusCode.OK, new { OrderStatusId = orderStatusId, Message = this.orderLocalizer.GetString("OrderStatusUpdatedSuccessfully").Value });
        }

        [HttpPost]
        public async Task<IActionResult> Item([FromBody] UpdateOrderItemStatusRequestModel request)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.ordersRepository.UpdateOrderItemStatusAsync(token, language, request.Id, request.OrderItemStatusId, request.OrderItemStatusChangeComment);

            var orderItemStatusChanges = await this.ordersRepository.GetOrderItemStatusesAsync(token, language, request.Id);

            return this.StatusCode((int)HttpStatusCode.OK, new { StatusChanges = orderItemStatusChanges.StatusChanges, Message = this.orderLocalizer.GetString("OrderStatusUpdatedSuccessfully").Value });
        }
    }
}
