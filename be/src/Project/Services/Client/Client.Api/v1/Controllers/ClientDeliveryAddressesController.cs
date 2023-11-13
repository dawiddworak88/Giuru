using Client.Api.Services.Addresses;
using Client.Api.ServicesModels.Addresses;
using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Client.Api.Validators.Addresses;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    public class ClientDeliveryAddressesController : BaseApiController
    {
        private readonly IClientAddressesService _clientAddressesService;

        public ClientDeliveryAddressesController(
            IClientAddressesService clientAddressesService) 
        {
            _clientAddressesService = clientAddressesService;
        }

        /// <summary>
        /// Gets list of clients addresses.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of clients addresses.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult Get(Guid? clientId, string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetClientAddressesServiceModel
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
            
            var clientsAddresses = _clientAddressesService.Get(serviceModel);

            if (clientsAddresses is not null)
            {
                var response = new PagedResults<IEnumerable<ClientAddressResponseModel>>(clientsAddresses.Total, clientsAddresses.PageSize)
                {
                    Data = clientsAddresses.Data.OrEmptyIfNull().Select(x => new ClientAddressResponseModel
                    {
                        Id = x.Id,
                        CountryId = x.CountryId,
                        ClientId = x.ClientId,
                        ClientName = x.ClientName,
                        Company = x.Company,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        City = x.City,
                        Region = x.Region,
                        Street = x.Street,
                        PostCode = x.PostCode,
                        PhoneNumber = x.PhoneNumber,
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedDate = x.CreatedDate
                    })
                };

                return StatusCode((int)HttpStatusCode.OK, response);
            }

            return StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets client address by id.
        /// </summary>
        /// <param name="id">The client address id.</param>
        /// <returns>The client address.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetById(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetClientAddressServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            var validator = new GetClientAddressModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var clientAddress = await _clientAddressesService.GetAsync(serviceModel);

                if (clientAddress is not null)
                {
                    var response = new ClientAddressResponseModel
                    {
                        Id = clientAddress.Id,
                        CountryId = clientAddress.CountryId,
                        ClientId = clientAddress.ClientId,
                        ClientName = clientAddress.ClientName,
                        Company = clientAddress.Company,
                        FirstName = clientAddress.FirstName, 
                        LastName = clientAddress.LastName,
                        City = clientAddress.City,
                        Region = clientAddress.Region,
                        Street = clientAddress.Street,
                        PostCode = clientAddress.PostCode,
                        PhoneNumber = clientAddress.PhoneNumber,
                        LastModifiedDate = clientAddress.LastModifiedDate,
                        CreatedDate = clientAddress.CreatedDate
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
        /// Creates or updates client address (if client address id is set).
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The client address id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(ClientAddressRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateClientAddressServiceModel
                {
                    Id = request.Id,
                    CountryId = request.CountryId,
                    ClientId = request.ClientId,
                    Company = request.Company,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    City = request.City,
                    Region = request.Region,
                    PostCode = request.PostCode,
                    Street = request.Street,
                    PhoneNumber = request.PhoneNumber,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateClientAddressModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var clientAddressId = await _clientAddressesService.UpdateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = clientAddressId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateClientAddressServiceModel
                {
                    CountryId = request.CountryId,
                    ClientId = request.ClientId,
                    Company = request.Company,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    City = request.City,
                    PostCode = request.PostCode,
                    Street = request.Street,
                    Region = request.Region,
                    PhoneNumber = request.PhoneNumber,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateClientAddressModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var clientAddressId = await _clientAddressesService.CreateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.Created, new { Id = clientAddressId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Delete client address by id.
        /// </summary>
        /// <param name="id">The client address id.</param>
        /// <returns>OK.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new DeleteClientAddressServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            var validator = new DeleteClientAddressModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _clientAddressesService.DeleteAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
