using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Api.Services.OrderAttributeValues;
using Ordering.Api.ServicesModels;
using Ordering.Api.v1.RequestModels;
using Ordering.Api.v1.ResponseModels;
using Ordering.Api.Validators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ordering.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderAttributeValuesController : BaseApiController
    {
        private readonly IOrderAttributeValuesService _orderAttributeValuesService;

        public OrderAttributeValuesController(IOrderAttributeValuesService orderAttributeValuesService)
        {
            _orderAttributeValuesService = orderAttributeValuesService;
        }

        /// <summary>
        /// Gets list of attributes values.
        /// </summary>
        /// <param name="orderId">The order id.</param>
        /// <param name="orderItemId">The order item id that is available for part of the order.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of attributes values.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult Get(Guid? orderId, Guid? orderItemId, string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetOrderAttributeValuesServiceModel
            {
                OrderId = orderId,
                OrderItemId = orderItemId,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var attributeValues = _orderAttributeValuesService.Get(serviceModel);

            if (attributeValues is not null)
            {
                var response = new PagedResults<IEnumerable<OrderAttributeValueResponseModel>>(attributeValues.Total, attributeValues.PageSize)
                {
                    Data = attributeValues.Data.OrEmptyIfNull().Select(x => new OrderAttributeValueResponseModel
                    {
                        Id = x.Id,
                        Value = x.Value,
                        AttributeId = x.AttributeId,
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedDate = x.CreatedDate
                    })
                };

                return StatusCode((int)HttpStatusCode.OK, response);
            }

            return StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }

        // <summary>
        /// Batch create or updates attribute values
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>OK.</returns>
        [HttpPost("Batch"), MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Batch([FromBody] BatchOrderAttributeValuesRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new CreateBatchOrderAttributeValuesServiceModel
            {
                OrderId = request.OrderId,
                Values = request.Values.OrEmptyIfNull().Select(x => new CreateOrderAttributeValueServiceModel
                {
                    AttributeId = x.AttributeId,
                    OrderItemId = x.OrderItemId,
                    Value = x.Value
                }),
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new CreateBatchOrderAttributeValuesModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _orderAttributeValuesService.BatchAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
