using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.OrderAttributeOptions;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class OrderAttributeOptionsApiController : BaseApiController
    {
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IOrderAttributeOptionsRepository _orderAttributeOptionsRepository;

        public OrderAttributeOptionsApiController(
            IStringLocalizer<OrderResources> orderLocalizer,
            IOrderAttributeOptionsRepository orderAttributeOptionsRepository)
        {
            _orderLocalizer = orderLocalizer;
            _orderAttributeOptionsRepository = orderAttributeOptionsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] OrderAttributeOptionRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var attributeOptionId = await _orderAttributeOptionsRepository.SaveAsync(token, language, model.Id, model.Name, model.AttributeId);

            return StatusCode((int)HttpStatusCode.OK, new { Id = attributeOptionId, Message = _orderLocalizer.GetString("OrderAttributeOptionSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _orderAttributeOptionsRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = _orderLocalizer.GetString("OrderAttributeOptionDeletedSuccessfully").Value });
        }

        [HttpGet]
        [Route("[area]/[controller]/{attributeId}")]
        public async Task<IActionResult> Get(Guid? attributeId, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var attributeOptions = await _orderAttributeOptionsRepository.GetAsync(
                token, 
                language, 
                attributeId, 
                searchTerm, 
                pageIndex, 
                itemsPerPage, 
                $"{nameof(OrderAttributeOption.CreatedDate)} desc");

            return StatusCode((int)HttpStatusCode.OK, attributeOptions);
        }
    }
}
