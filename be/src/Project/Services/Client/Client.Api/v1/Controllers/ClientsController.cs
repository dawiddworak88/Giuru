using Client.Api.Services.Clients;
using Client.Api.ServicesModels.Clients;
using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Client.Api.Validators.Clients;
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
    public class ClientsController : BaseApiController
    {
        private readonly IClientsService _clientsService;

        public ClientsController(IClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        /// <summary>
        /// Gets list of clients.
        /// </summary>
        /// <param name="ids">The client ids.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of clients.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult Get(string ids, string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var clientIds = ids.ToEnumerableGuidIds();

            if (clientIds is not null)
            {
                var serviceModel = new GetClientsByIdsServiceModel
                {
                    Ids = clientIds,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new GeClientsByIdsModelValidator();
                var validationResult = validator.Validate(serviceModel);

                if (validationResult.IsValid)
                {
                    var clients = _clientsService.GetByIds(serviceModel);

                    if (clients is not null)
                    {
                        var response = new PagedResults<IEnumerable<ClientResponseModel>>(clients.Total, clients.PageSize)
                        {
                            Data = clients.Data.OrEmptyIfNull().Select(x => new ClientResponseModel
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Email = x.Email,
                                CommunicationLanguage = x.CommunicationLanguage,
                                CountryId = x.CountryId,
                                PreferedCurrencyId = x.PreferedCurrencyId,
                                OrganisationId = x.OrganisationId,
                                PhoneNumber = x.PhoneNumber,
                                IsDisabled = x.IsDisabled,
                                ClientGroupIds = x.ClientGroupIds,
                                ClientManagerIds = x.ClientManagerIds,
                                DefaultDeliveryAddressId = x.DefaultDeliveryAddressId,
                                DefaultBillingAddressId = x.DefaultBillingAddressId,
                                LastModifiedDate = x.LastModifiedDate,
                                CreatedDate = x.CreatedDate
                            })
                        };

                        return StatusCode((int)HttpStatusCode.OK, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new GetClientsServiceModel
                {
                    Language = CultureInfo.CurrentCulture.Name,
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new GetClientsModelValidator();
                var validationResult = validator.Validate(serviceModel);

                if (validationResult.IsValid)
                {
                    var clients = _clientsService.Get(serviceModel);

                    if (clients is not null)
                    {
                        var response = new PagedResults<IEnumerable<ClientResponseModel>>(clients.Total, clients.PageSize)
                        {
                            Data = clients.Data.OrEmptyIfNull().Select(x => new ClientResponseModel
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Email = x.Email,
                                CountryId = x.CountryId,
                                PreferedCurrencyId = x.PreferedCurrencyId,
                                OrganisationId = x.OrganisationId,
                                CommunicationLanguage = x.CommunicationLanguage,
                                PhoneNumber = x.PhoneNumber,
                                IsDisabled = x.IsDisabled,
                                ClientGroupIds = x.ClientGroupIds,
                                ClientManagerIds = x.ClientManagerIds,
                                DefaultDeliveryAddressId = x.DefaultDeliveryAddressId,
                                DefaultBillingAddressId = x.DefaultBillingAddressId,
                                LastModifiedDate = x.LastModifiedDate,
                                CreatedDate = x.CreatedDate
                            })
                        };

                        return StatusCode((int)HttpStatusCode.OK, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Gets client by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The client.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetClientServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetClientModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var client = await _clientsService.GetAsync(serviceModel);

                if (client is not null)
                {
                    var response = new ClientResponseModel
                    {
                        Id = client.Id,
                        Email = client.Email,
                        Name = client.Name,
                        CommunicationLanguage = client.CommunicationLanguage,
                        CountryId = client.CountryId,
                        PreferedCurrencyId = client.PreferedCurrencyId,
                        OrganisationId = client.OrganisationId,
                        PhoneNumber = client.PhoneNumber,
                        IsDisabled = client.IsDisabled,
                        ClientGroupIds = client.ClientGroupIds,
                        ClientManagerIds = client.ClientManagerIds,
                        DefaultDeliveryAddressId = client.DefaultDeliveryAddressId,
                        DefaultBillingAddressId = client.DefaultBillingAddressId,
                        LastModifiedDate = client.LastModifiedDate,
                        CreatedDate = client.CreatedDate
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
        /// Gets client by email.
        /// </summary>
        /// <param name="email">The client email.</param>
        /// <returns>The client.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("email/{email}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetClientByEmailServiceModel
            {
                Email = email,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetClientByEmailModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var client = await _clientsService.GetByEmailAsync(serviceModel);

                if (client is not null)
                {
                    var response = new ClientResponseModel
                    {
                        Id = client.Id,
                        Email = client.Email,
                        Name = client.Name,
                        CommunicationLanguage = client.CommunicationLanguage,
                        CountryId = client.CountryId,
                        PreferedCurrencyId = client.PreferedCurrencyId,
                        OrganisationId = client.OrganisationId,
                        PhoneNumber = client.PhoneNumber,
                        IsDisabled = client.IsDisabled,
                        ClientGroupIds = client.ClientGroupIds,
                        ClientManagerIds = client.ClientManagerIds,
                        DefaultDeliveryAddressId = client.DefaultDeliveryAddressId,
                        DefaultBillingAddressId = client.DefaultBillingAddressId,
                        LastModifiedDate = client.LastModifiedDate,
                        CreatedDate = client.CreatedDate
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
        /// Gets client by id.
        /// </summary>
        /// <returns>The client.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("organisation")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetByOrganisation()
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var serviceModel = new GetClientByOrganisationServiceModel
            {
                Id = GuidHelper.ParseNullable(sellerClaim?.Value),
            };

            var validator = new GetClientByOrganisationModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var client = await _clientsService.GetByOrganisationAsync(serviceModel);

                if (client is not null)
                {
                    var response = new ClientResponseModel
                    {
                        Id = client.Id,
                        Email = client.Email,
                        Name = client.Name,
                        CommunicationLanguage = client.CommunicationLanguage,
                        CountryId = client.CountryId,
                        PreferedCurrencyId = client.PreferedCurrencyId,
                        OrganisationId = client.OrganisationId,
                        PhoneNumber = client.PhoneNumber,
                        IsDisabled = client.IsDisabled,
                        ClientGroupIds = client.ClientGroupIds,
                        ClientManagerIds = client.ClientManagerIds,
                        DefaultDeliveryAddressId = client.DefaultDeliveryAddressId,
                        DefaultBillingAddressId = client.DefaultBillingAddressId,
                        LastModifiedDate = client.LastModifiedDate,
                        CreatedDate = client.CreatedDate
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
        /// Creates or updates client (if client id is set).
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The client id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(ClientRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateClientServiceModel
                {
                    Id = request.Id,
                    Name = request.Name,
                    Email = request.Email,
                    CountryId = request.CountryId,
                    PreferedCurrencyId = request.PreferedCurrencyId,
                    CommunicationLanguage = request.CommunicationLanguage,
                    PhoneNumber = request.PhoneNumber,
                    IsDisabled = request.IsDisabled,
                    ClientOrganisationId = request.OrganisationId,
                    ClientGroupIds = request.ClientGroupIds,
                    ClientManagerIds = request.ClientManagerIds,
                    DefaultDeliveryAddressId = request.DefaultDeliveryAddressId,
                    DefaultBillingAddressId = request.DefaultBillingAddressId,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateClientModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var client = await _clientsService.UpdateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = client.Id });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateClientServiceModel
                {
                    Name = request.Name,
                    Email = request.Email,
                    CountryId = request.CountryId,
                    PreferedCurrencyId = request.PreferedCurrencyId,
                    CommunicationLanguage = request.CommunicationLanguage,
                    PhoneNumber = request.PhoneNumber,
                    ClientOrganisationId = request.OrganisationId,
                    ClientGroupIds = request.ClientGroupIds,
                    ClientManagerIds = request.ClientManagerIds,
                    DefaultDeliveryAddressId = request.DefaultDeliveryAddressId,
                    DefaultBillingAddressId = request.DefaultBillingAddressId,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateClientModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var client = await _clientsService.CreateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.Created, new { Id = client.Id });
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
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new DeleteClientServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteClientModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _clientsService.DeleteAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}