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
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ApiControllers
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
    }
}
