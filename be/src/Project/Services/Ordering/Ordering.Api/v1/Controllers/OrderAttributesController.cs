using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Api.Services.OrderAttributes;
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
    public class OrderAttributesController : BaseApiController
    {
        private readonly IOrderAttributesService _orderAttributesService;

        public OrderAttributesController(IOrderAttributesService orderAttributesService)
        {
            _orderAttributesService = orderAttributesService;
        }

        /// <summary>
        /// Gets list of order attributes.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of order attributes.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult Get(string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetOrderAttributesServiceModel
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var orderAttributes = _orderAttributesService.Get(serviceModel);

            if (orderAttributes is not null)
            {
                var response = new PagedResults<IEnumerable<OrderAttributeResponseModel>>(orderAttributes.Total, orderAttributes.PageSize)
                {
                    Data = orderAttributes.Data.OrEmptyIfNull().Select(x => new OrderAttributeResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Type = x.Type,
                        IsRequired = x.IsRequired,
                        IsOrderItemAttribute = x.IsOrderItemAttribute,
                        Options = x.OrderAttributeOptions.Select(y => new AttributeOptionResponseModel
                        {
                            Name = y.Name,
                            Value = y.Value,
                        }),
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedDate = x.CreatedDate
                    })
                };

                return StatusCode((int)HttpStatusCode.OK, response);
            }

            return StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates or updates order attribute (if order attribute id is set).
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The order attribute id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save([FromBody] OrderAttributeRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateOrderAttributeServiceModel
                {
                    Id = request.Id,
                    Name = request.Name,
                    Type = request.Type,
                    IsRequired = request.IsRequired,
                    IsOrderItemAttribute = request.IsOrderItemAttribute,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateOrderAttributeModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var orderAttributeId = await _orderAttributesService.UpdateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = orderAttributeId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateOrderAttributeServiceModel
                {
                    Name = request.Name,
                    Type = request.Type,
                    IsRequired = request.IsRequired,
                    IsOrderItemAttribute = request.IsOrderItemAttribute,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateOrderAttributeModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var orderAttributeId = await _orderAttributesService.CreateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = orderAttributeId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Get order attribute by id.
        /// </summary>
        /// <param name="id">The attribute id.</param>
        /// <returns>The attribute.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetOrderAttribute(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetOrderAttributeServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            var validator = new GetOrderAttributeModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var orderAttribute = await _orderAttributesService.GetAsync(serviceModel);

                if (orderAttribute is not null)
                {
                    var response = new OrderAttributeResponseModel
                    {
                        Id = orderAttribute.Id,
                        Name = orderAttribute.Name,
                        Type = orderAttribute.Type,
                        IsRequired = orderAttribute.IsRequired,
                        IsOrderItemAttribute = orderAttribute.IsOrderItemAttribute,
                        Options = orderAttribute.OrderAttributeOptions.Select(x => new AttributeOptionResponseModel
                        {
                            Name = x.Name,
                            Value = x.Value
                        }),
                        LastModifiedDate = orderAttribute.LastModifiedDate,
                        CreatedDate = orderAttribute.CreatedDate
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.NoContent);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Delete order attribute by id.
        /// </summary>
        /// <param name="id">The order attribute id.</param>
        /// <returns>OK.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> DeleteFieldDefinition(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new DeleteOrderAttributeServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            var validator = new DeleteOrderAttributeModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _orderAttributesService.DeleteAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
