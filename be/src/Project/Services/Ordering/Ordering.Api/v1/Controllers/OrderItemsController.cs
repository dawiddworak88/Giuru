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
using Foundation.Extensions.ExtensionMethods;
using Ordering.Api.v1.RequestModels;
using Ordering.Api.ServicesModels.OrderItems;
using Ordering.Api.Validators.OrderItems;

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

        /// <summary>
        /// Gets order item statuses history by order item id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The order item statuses.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("statuschanges/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetOrderItemStatuses(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetOrderItemStatusChangesServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            var validator = new GetOrderItemStatusChangesModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var statusChanges = await _orderItemsService.GetStatusChangesAsync(serviceModel);

                if (statusChanges is not null)
                {
                    var response = new OrderItemStatusChangesResponseModel
                    {
                        OrderItemId = statusChanges.OrderItemId,
                        StatusChanges = statusChanges.OrderItemStatusChanges.OrEmptyIfNull().Select(x => new OrderItemStatusChangeResponseModel
                        {
                            OrderItemStateId = x.OrderItemStateId,
                            OrderItemStatusId = x.OrderItemStatusId,
                            OrderItemStatusName = x.OrderItemStatusName,
                            OrderItemStatusChangeComment = x.OrderItemStatusChangeComment,
                            CreatedDate = x.CreatedDate
                        })
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        ///  Updates the order item status.
        /// </summary>
        /// <returns>The updated order item status.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [Route("status")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Status(UpdateOrderItemStatusRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new UpdateOrderItemStatusServiceModel
            {
                Id = request.Id,
                OrderItemStatusId = request.OrderItemStatusId,
                OrderItemStatusChangeComment = request.ExpectedDateOfProductOnStock,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            var validator = new UpdateOrderItemStatusModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _orderItemsService.UpdateStatusAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
