using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Api.Services.OrderAttributeOptions;
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
    public class OrderAttributeOptionsController : BaseApiController
    {
        private readonly IOrderAttributeOptionsService _orderAttributeOptionsService;

        public OrderAttributeOptionsController(
            IOrderAttributeOptionsService orderAttributeOptionsService)
        {
            _orderAttributeOptionsService = orderAttributeOptionsService;
        }

        /// <summary>
        /// Delete attribute option by id.
        /// </summary>
        /// <param name="id">The attribute option id.</param>
        /// <returns>OK.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new DeleteOrderAttributeOptionServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            var validator = new DeleteOrderAttributeOptionModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _orderAttributeOptionsService.DeleteAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets list of attributes options.
        /// </summary>
        /// <param name="attributeId">The attribute id.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of attributes options.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult Get(Guid? attributeId, string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetOrderAttributeOptionsServiceModel
            {
                AttributeId = attributeId,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var attributeOptions = _orderAttributeOptionsService.Get(serviceModel);

            if (attributeOptions is not null)
            {
                var response = new PagedResults<IEnumerable<OrderAttributeOptionResponseModel>>(attributeOptions.Total, attributeOptions.PageSize)
                {
                    Data = attributeOptions.Data.OrEmptyIfNull().Select(x => new OrderAttributeOptionResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Value = x.Value,
                        AttributeId = x.OrderAttributeId,
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedDate = x.CreatedDate
                    })
                };

                return StatusCode((int)HttpStatusCode.OK, response);
            }

            return StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Get attribute option by id.
        /// </summary>
        /// <param name="id">The attribute option id.</param>
        /// <returns>OK.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetOption(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetOrderAttributeOptionServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            var validator = new GetOrderAttributeOptionModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var attributeOption = await _orderAttributeOptionsService.GetAsync(serviceModel);

                if (attributeOption is not null)
                {
                    var response = new OrderAttributeOptionResponseModel
                    {
                        Id = attributeOption.Id,
                        Name = attributeOption.Name,
                        Value = attributeOption.Value,
                        AttributeId = attributeOption.OrderAttributeId,
                        LastModifiedDate = attributeOption.LastModifiedDate,
                        CreatedDate = attributeOption.CreatedDate
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates or updates attribute option (if attribute option id is set).
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The attribute option id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save([FromBody] OrderAttributeOptionRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateOrderAttributeOptionServiceModel
                {
                    Id = request.Id,
                    Name = request.Name,
                    OrderAttributeId = request.AttributeId,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateOrderAttributeOptionModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var attributeOptionId = await _orderAttributeOptionsService.UpdateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = attributeOptionId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateOrderAttributeOptionServiceModel
                {
                    Name = request.Name,
                    OrderAttributeId = request.AttributeId,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateOrderAttributeOptionModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var attributeOptionId = await _orderAttributeOptionsService.CreateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = attributeOptionId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}
