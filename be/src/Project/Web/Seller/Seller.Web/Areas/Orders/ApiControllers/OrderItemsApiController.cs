using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.Definitions;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.OrderAttributeValues;
using Seller.Web.Areas.Orders.Repositories.OrderItems;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ApiControllers
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

            await _orderItemsRepository.UpdateStatusAsync(token, language, request.Id, cancelStatusId, null);

            var orderItemStatusChanges = await _orderItemsRepository.GetStatusChangesAsync(token, language, request.Id);

            return StatusCode((int)HttpStatusCode.OK, new { OrderItemStatus = cancelStatusId, StatusChanges = orderItemStatusChanges.StatusChanges, Message = _orderLocalizer.GetString("SuccessfullyCanceledOrder").Value });
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveOrderItemRequestModel request)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            if (request.AttributesValues is not null && 
                request.AttributesValues.Any())
            {
                await _orderAttributeValuesRepository.BatchAsync(token, language,
                    request.AttributesValues.Select(x => new ApiOrderAttributeValue
                    {
                        OrderId = request.OrderId,
                        OrderItemId = request.Id,
                        AttributeId = x.AttributeId,
                        Value = x.Value
                    }));
            }

            var orderItemStatusChanges = new OrderItemStatusChanges();

            if (request.LastOrderItemStatusId != request.NewOrderItemStatusId)
            {
                await _orderItemsRepository.UpdateStatusAsync(token, language, request.Id, request.NewOrderItemStatusId, request.ExpectedDateOfProductOnStock);

                orderItemStatusChanges = await _orderItemsRepository.GetStatusChangesAsync(token, language, request.Id);
            }

            return StatusCode((int)HttpStatusCode.OK, new { StatusChanges = orderItemStatusChanges.StatusChanges, Message = _orderLocalizer.GetString("OrderStatusUpdatedSuccessfully").Value });
        }
    }
}
