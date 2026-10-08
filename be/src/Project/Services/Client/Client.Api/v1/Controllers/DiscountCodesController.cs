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
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private const string ValidStatus = "Valid";
        private const string NotApplicableStatus = "NotApplicable";

        private readonly IDiscountCodesService _discountCodesService;

        public DiscountCodesController(IDiscountCodesService discountCodesService)
        {
            _discountCodesService = discountCodesService;
        }

        /// <summary>
        /// Gets the discount codes of the seller.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of discount codes.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Authorize(Roles = "Seller")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
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
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GetOrganisationId()
            };

            var validationResult = await new GetDiscountCodesModelValidator().ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var discountCodes = _discountCodesService.Get(serviceModel);

                if (discountCodes is not null)
                {
                    var response = new PagedResults<IEnumerable<DiscountCodeResponseModel>>(discountCodes.Total, discountCodes.PageSize)
                    {
                        Data = discountCodes.Data.OrEmptyIfNull().Select(x => new DiscountCodeResponseModel
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Description = x.Description,
                            IsDisabled = x.IsDisabled,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        })
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets a discount code by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The discount code.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id:guid}")]
        [Authorize(Roles = "Seller")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DiscountCodeResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var serviceModel = new GetDiscountCodeServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GetOrganisationId()
            };

            var validationResult = await new GetDiscountCodeModelValidator().ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var discountCode = await _discountCodesService.GetAsync(serviceModel);

                if (discountCode is not null)
                {
                    return StatusCode((int)HttpStatusCode.OK, new DiscountCodeResponseModel
                    {
                        Id = discountCode.Id,
                        Code = discountCode.Code,
                        Description = discountCode.Description,
                        IsDisabled = discountCode.IsDisabled,
                        LastModifiedDate = discountCode.LastModifiedDate,
                        CreatedDate = discountCode.CreatedDate
                    });
                }

                return StatusCode((int)HttpStatusCode.NoContent);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates or updates a discount code (if the id is set). Only the description and the disabled flag can change.
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The discount code id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [Authorize(Roles = "Seller")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(DiscountCodeRequestModel request)
        {
            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateDiscountCodeServiceModel
                {
                    Id = request.Id,
                    Description = request.Description,
                    IsDisabled = request.IsDisabled,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GetOrganisationId()
                };

                var validationResult = await new UpdateDiscountCodeModelValidator().ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var id = await _discountCodesService.UpdateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = id });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateDiscountCodeServiceModel
                {
                    Code = request.Code,
                    Description = request.Description,
                    IsDisabled = request.IsDisabled,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GetOrganisationId()
                };

                var validationResult = await new CreateDiscountCodeModelValidator().ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var id = await _discountCodesService.CreateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = id });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Deletes a discount code by id. Its assignments to clients are deactivated with it.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>OK.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id:guid}")]
        [Authorize(Roles = "Seller")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var serviceModel = new DeleteDiscountCodeServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GetOrganisationId()
            };

            var validationResult = await new DeleteDiscountCodeModelValidator().ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _discountCodesService.DeleteAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Answers whether a discount code can be applied for a client. A code that cannot be applied is a
        /// 200 answer, not an error. Buyers can only ask about their own client and are never told why a code
        /// is not applicable.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="clientId">The client id.</param>
        /// <returns>The validation answer.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("validation")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DiscountCodeValidationResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Validate(string code, string clientId)
        {
            var isSeller = User.IsInRole(AccountConstants.Roles.Seller);

            // Parsed here rather than bound, so malformed input is a 422 like every other invalid input
            // instead of the automatic 400 of [ApiController] model binding.
            var serviceModel = new ValidateDiscountCodeServiceModel
            {
                Code = code,
                ClientId = Guid.TryParse(clientId, out var parsedClientId) ? parsedClientId : null,
                IsSeller = isSeller,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GetOrganisationId()
            };

            var validationResult = await new ValidateDiscountCodeModelValidator().ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var validation = await _discountCodesService.ValidateAsync(serviceModel);

                if (validation.IsValid)
                {
                    return StatusCode((int)HttpStatusCode.OK, new DiscountCodeValidationResponseModel
                    {
                        Status = ValidStatus,
                        DiscountCode = validation.DiscountCode
                    });
                }

                return StatusCode((int)HttpStatusCode.OK, new DiscountCodeValidationResponseModel
                {
                    Status = isSeller ? validation.Status.ToString() : NotApplicableStatus
                });
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        private Guid? GetOrganisationId()
        {
            var organisationClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value;

            return Guid.TryParse(organisationClaim, out var organisationId) ? organisationId : null;
        }
    }
}
