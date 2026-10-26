using Asp.Versioning;
using Client.Api.Services.DiscountCodes;
using Client.Api.ServicesModels.DiscountCodes;
using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Client.Api.Validators.DiscountCodes;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class DiscountCodesController : BaseApiController
    {
        private readonly IDiscountCodesService _discountCodesService;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;

        public DiscountCodesController(
            IDiscountCodesService discountCodesService,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            _discountCodesService = discountCodesService;
            _clientLocalizer = clientLocalizer;
        }

        /// <summary>
        /// Gets list of discount codes.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of discount codes.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<DiscountCodeResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var serviceModel = new GetDiscountCodesServiceModel
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name,
                Username = GetUsername(),
                OrganisationId = GetOrganisationId()
            };

            var validationResult = await new GetDiscountCodesModelValidator().ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var discountCodes = await _discountCodesService.GetAsync(serviceModel);

                var response = new PagedResults<IEnumerable<DiscountCodeResponseModel>>(discountCodes.Total, discountCodes.PageSize)
                {
                    Data = discountCodes.Data.OrEmptyIfNull().Select(MapToResponse)
                };

                return this.StatusCode((int)HttpStatusCode.OK, response);
            }

            throw ToValidationException(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        /// <summary>
        /// Checks whether a discount code is a valid discount code for a client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="code">The requested code text.</param>
        /// <returns>The stored spelling of the code, or not found without a reason.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("eligibility")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ValidDiscountCodeResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetEligibility(Guid? clientId, string code)
        {
            var serviceModel = new GetValidDiscountCodeServiceModel
            {
                ClientId = clientId,
                Code = code,
                Language = CultureInfo.CurrentCulture.Name,
                Username = GetUsername(),
                OrganisationId = GetOrganisationId()
            };

            var validationResult = await new GetValidDiscountCodeModelValidator().ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var validCode = await _discountCodesService.GetValidCodeAsync(serviceModel);

                if (validCode is not null)
                {
                    return this.StatusCode((int)HttpStatusCode.OK, new ValidDiscountCodeResponseModel { Code = validCode });
                }

                return this.StatusCode((int)HttpStatusCode.NotFound);
            }

            throw ToValidationException(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        /// <summary>
        /// Gets discount code by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The discount code.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DiscountCodeResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var serviceModel = new GetDiscountCodeServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = GetUsername(),
                OrganisationId = GetOrganisationId()
            };

            var validationResult = await new GetDiscountCodeModelValidator().ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var discountCode = await _discountCodesService.GetAsync(serviceModel);

                if (discountCode is not null)
                {
                    return this.StatusCode((int)HttpStatusCode.OK, MapToResponse(discountCode));
                }

                return this.StatusCode((int)HttpStatusCode.NoContent);
            }

            throw ToValidationException(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        /// <summary>
        /// Creates or updates discount code (if id is set).
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The discount code id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(DiscountCodeRequestModel request)
        {
            var serviceModel = new SaveDiscountCodeServiceModel
            {
                Id = request.Id,
                Code = request.Code?.Trim(),
                Description = request.Description,
                IsDisabled = request.IsDisabled,
                ClientIds = request.ClientIds,
                Language = CultureInfo.CurrentCulture.Name,
                Username = GetUsername(),
                OrganisationId = GetOrganisationId()
            };

            var validationResult = await new SaveDiscountCodeModelValidator(_clientLocalizer).ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var discountCodeId = await _discountCodesService.SaveAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK, new { Id = discountCodeId });
            }

            throw ToValidationException(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        /// <summary>
        /// Deletes discount code by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>OK.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var serviceModel = new DeleteDiscountCodeServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = GetUsername(),
                OrganisationId = GetOrganisationId()
            };

            var validationResult = await new DeleteDiscountCodeModelValidator().ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _discountCodesService.DeleteAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw ToValidationException(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        private string GetUsername()
        {
            return this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        }

        private Guid? GetOrganisationId()
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            return GuidHelper.ParseNullable(sellerClaim?.Value);
        }

        private static CustomException ToValidationException(IEnumerable<string> errorMessages)
        {
            return new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, errorMessages), (int)HttpStatusCode.UnprocessableEntity);
        }

        private static DiscountCodeResponseModel MapToResponse(DiscountCodeServiceModel discountCode)
        {
            return new DiscountCodeResponseModel
            {
                Id = discountCode.Id,
                Code = discountCode.Code,
                Description = discountCode.Description,
                IsDisabled = discountCode.IsDisabled,
                ClientsCount = discountCode.ClientsCount,
                ClientIds = discountCode.ClientIds,
                LastModifiedDate = discountCode.LastModifiedDate,
                CreatedDate = discountCode.CreatedDate
            };
        }
    }
}
