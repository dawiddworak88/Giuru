using Client.Api.Services.FieldValues;
using Client.Api.ServicesModels.FieldValues;
using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Client.Api.Validators.FieldValues;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
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
    public class ClientFieldValuesController : BaseApiController
    {
        private readonly IClientFieldValuesService _clientFieldValuesService;

        public ClientFieldValuesController(
            IClientFieldValuesService clientFieldValuesService)
        {
            _clientFieldValuesService = clientFieldValuesService;
        }

        /// <summary>
        /// Gets list of field values.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of fields definitions.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult Get(Guid? clientId, string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetClientFieldValuesServiceModel
            {
                ClientId = clientId,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var clientFieldsValues = _clientFieldValuesService.Get(serviceModel);

            if (clientFieldsValues is not null)
            {
                var response = new PagedResults<IEnumerable<ClientFieldValueResponseModel>>(clientFieldsValues.Total, clientFieldsValues.PageSize)
                {
                    Data = clientFieldsValues.Data.OrEmptyIfNull().Select(x => new ClientFieldValueResponseModel
                    {
                        Id = x.Id,
                        FieldValue = x.FieldValue,
                        FieldDefinitionId = x.FieldDefinitionId,
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedDate = x.CreatedDate
                    })
                };

                return StatusCode((int)HttpStatusCode.OK, response);
            }

            return StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates or updates client field values
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>OK.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save([FromBody] ClientFieldValuesRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new CreateClientFieldValuesServiceModel
            {
                ClientId = request.ClientId,
                FieldsValues = request.FieldsValues.OrEmptyIfNull().Select(x => new CreateClientFieldValueServiceModel
                {
                    FieldDefinitionId = x.FieldDefinitionId,
                    FieldValue = x.FieldValue
                }),
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new CreateClientFieldValuesModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _clientFieldValuesService.CreateAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
