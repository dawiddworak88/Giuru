using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Api.Services.OrderItems;
using Ordering.Api.ServicesModels;
using Ordering.Api.v1.ResponseModels;
using Ordering.Api.Validators;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using Foundation.Account.Definitions;

namespace Ordering.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderItemsController : BaseApiController
    {
        private readonly IOrderItemsService _orderItemsService;

        public OrderItemsController(
            IOrderItemsService orderItemsService)
        {
            _orderItemsService = orderItemsService;
        }

        /// <summary>
        /// Gets order item by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The order item.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetOrderItemServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            var validator = new GetOrderItemModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var orderItem = await _orderItemsService.GetAsync(serviceModel);

                if (orderItem is not null)
                {
                    var response = new OrderItemResponseModel
                    {
                        Id = orderItem.Id,
                        OrderId = orderItem.OrderId,
                        ProductId = orderItem.ProductId,
                        ProductSku = orderItem.ProductSku,
                        ProductName = orderItem.ProductName,
                        PictureUrl = orderItem.PictureUrl,
                        Quantity = orderItem.Quantity,
                        StockQuantity = orderItem.StockQuantity,
                        OutletQuantity = orderItem.OutletQuantity,
                        ExternalReference = orderItem.ExternalReference,
                        MoreInfo = orderItem.MoreInfo,
                        OrderItemStateId = orderItem.OrderItemStateId,
                        OrderItemStatusId = orderItem.OrderItemStatusId,
                        OrderItemStatusName = orderItem.OrderItemStatusName,
                        OrderItemStatusChangeComment = orderItem.OrderItemStatusChangeComment,
                        LastOrderItemStatusChangeId = orderItem.LastOrderItemStatusChangeId,
                        LastModifiedDate = orderItem.LastModifiedDate,
                        CreatedDate = orderItem.CreatedDate
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
