using Client.Api.Services.Fields;
using Client.Api.ServicesModels.Fields;
using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Client.Api.Validators.Feilds;
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
    public class ClientFieldsController : BaseApiController
    {
        private readonly IClientFieldsService _clientFieldsService;

        public ClientFieldsController(
            IClientFieldsService clientFieldsService)
        {
            _clientFieldsService = clientFieldsService;
        }

        /// <summary>
        /// Gets list of fields definitions.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of fields definitions.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult Get(string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetClientFieldsServiceModel
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var clientFieldsDefinitions = _clientFieldsService.Get(serviceModel);

            if (clientFieldsDefinitions is not null)
            {
                var response = new PagedResults<IEnumerable<ClientFieldResponseModel>>(clientFieldsDefinitions.Total, clientFieldsDefinitions.PageSize)
                {
                    Data = clientFieldsDefinitions.Data.OrEmptyIfNull().Select(x => new ClientFieldResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Type = x.Type,
                        IsRequired = x.IsRequired,
                        Options = x.Options.Select(y => new FieldOptionResponseModel
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
        /// Creates or updates client field (if client field id is set).
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The client field id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save([FromBody] ClientFieldRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateClientFieldServiceModel
                {
                    Id = request.Id,
                    Name = request.Name,
                    Type = request.Type,
                    IsRequired = request.IsRequired,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateClientFieldModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var clientFieldId = await _clientFieldsService.UpdateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = clientFieldId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateClientFieldServiceModel
                {
                    Name = request.Name,
                    Type = request.Type,
                    IsRequired = request.IsRequired,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateClientFieldModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var clientFieldId = await _clientFieldsService.CreateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = clientFieldId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Gets field definition by id.
        /// </summary>
        /// <param name="id">The client field definition id.</param>
        /// <returns>The field definition.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetFieldDefinition(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetClientFieldDefinitionServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            var validator = new GetClientFieldDefinitionModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var clientFieldDefinition = await _clientFieldsService.GetAsync(serviceModel);

                if (clientFieldDefinition is not null)
                {
                    var response = new ClientFieldResponseModel
                    {
                        Id = clientFieldDefinition.Id,
                        Name = clientFieldDefinition.Name,
                        Type = clientFieldDefinition.Type,
                        IsRequired = clientFieldDefinition.IsRequired,
                        Options = clientFieldDefinition.Options.Select(x => new FieldOptionResponseModel
                        {
                            Name = x.Name,
                            Value = x.Value
                        }),
                        LastModifiedDate = clientFieldDefinition.LastModifiedDate,
                        CreatedDate = clientFieldDefinition.CreatedDate
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
        /// Delete field definition by id.
        /// </summary>
        /// <param name="id">The client field definition id.</param>
        /// <returns>OK.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> DeleteFieldDefinition(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new DeleteClientFieldServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            var validator = new DeleteClientFieldDefinitionModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _clientFieldsService.DeleteAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
