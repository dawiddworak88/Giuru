using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Helpers;
using Identity.Api.v1.Areas.Clients.Models;
using Identity.Api.v1.Areas.Clients.RequestModels;
using Identity.Api.v1.Areas.Clients.Services;
using Identity.Api.v1.Areas.Clients.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Clients.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = "IsToken")]
    [ApiController]
    public class ClientsController : BaseApiController
    {
        private readonly IClientsService clientsService;

        public ClientsController(IClientsService clientsService)
        {
            this.clientsService = clientsService;
        }

        /// <summary>
        /// Gets list of clients.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of clients.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var serviceModel = new GetClientsModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetClientsModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var clients = await this.clientsService.GetAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK, clients);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets client by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The client.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var serviceModel = new GetClientModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetClientModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var client = await this.clientsService.GetAsync(serviceModel);

                return client != null ? this.StatusCode((int)HttpStatusCode.OK, client) : (IActionResult)this.StatusCode((int)HttpStatusCode.NotFound);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates or updates client (if client id is set).
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The client id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Save(ClientRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateClientModel
                {
                    Id = request.Id,
                    Name = request.Name,
                    Email = request.Email,
                    CommunicationLanguage = request.CommunicationLanguage,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateClientModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var client = await this.clientsService.UpdateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { Id = client.Id });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateClientModel
                {
                    Name = request.Name,
                    Email = request.Email,
                    CommunicationLanguage = request.CommunicationLanguage,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateClientModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var client = await this.clientsService.CreateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.Created, new { Id = client.Id });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Delete client by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>OK.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var serviceModel = new DeleteClientModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteClientModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.clientsService.DeleteAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
