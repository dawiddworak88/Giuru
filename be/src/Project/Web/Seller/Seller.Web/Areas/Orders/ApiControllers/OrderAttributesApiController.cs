using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.OrderAttributes;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class OrderAttributesApiController : BaseApiController
    {
        private readonly IOrderAttributesRepository _orderAttributesRepository;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;

        public OrderAttributesApiController(
            IOrderAttributesRepository orderAttributesRepository,
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            _orderAttributesRepository = orderAttributesRepository;
            _orderLocalizer = orderLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] OrderAttributeRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var orderAttributeId = await _orderAttributesRepository.SaveAsync(token, language, model.Id, model.Name, model.Type, model.IsOrderItemAttribute);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = orderAttributeId, Message = _orderLocalizer.GetString("OrderAttributeSavedSuccessfully").Value });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var orderAttributes = await _orderAttributesRepository.GetAsync(
                token, 
                language, 
                searchTerm, 
                pageIndex, 
                itemsPerPage, 
                $"{nameof(OrderAttribute.CreatedDate)}");

            return StatusCode((int)HttpStatusCode.OK, orderAttributes);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _orderAttributesRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = _orderLocalizer.GetString("OrderAttributeDeletedSuccessfully").Value });
        }
    }
}
