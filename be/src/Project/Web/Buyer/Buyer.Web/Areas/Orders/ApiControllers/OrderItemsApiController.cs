using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.Definitions;
using Buyer.Web.Areas.Orders.Repositories.OrderAttributeValues;
using Buyer.Web.Areas.Orders.Repositories.OrderItems;
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
    public class OrderItemsApiController : BaseApiController
    {
        private readonly IOrderItemsRepository _orderItemsRepository;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IOrderAttributeValuesRepository _orderAttributeValuesRepository;

        public OrderItemsApiController(
            IOrderItemsRepository orderItemsRepository,
            IStringLocalizer<OrderResources> orderLocalizer,
            IOrderAttributeValuesRepository orderAttributeValuesRepository)
        {
            _orderItemsRepository = orderItemsRepository;
            _orderLocalizer = orderLocalizer;
            _orderAttributeValuesRepository = orderAttributeValuesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Cancel([FromBody] UpdateOrderItemStatusRequestModel request)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;
            var cancelStatusId = OrdersConstants.OrderStatuses.CancelId;

            await _orderItemsRepository.UpdateStatusAsync(token, language, request.Id, cancelStatusId);

            var orderItemStatusChanges = await _orderItemsRepository.GetStatusChangesAsync(token, language, request.Id);

            return StatusCode((int)HttpStatusCode.OK, new { OrderItemStatus = cancelStatusId, StatusChanges = orderItemStatusChanges.StatusChanges, Message = _orderLocalizer.GetString("SuccessfullyCanceledOrder").Value });
        }
    }
}
