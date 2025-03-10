﻿using Client.Api.Services.Applications;
using Client.Api.ServicesModels.Applications;
using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Client.Api.Validators.Applications;
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
    public class ClientsApplicationsController : BaseApiController
    {
        private readonly IClientsApplicationsService _clientsApplicationService;

        public ClientsApplicationsController(
            IClientsApplicationsService clientsApplicationService)
        {
            _clientsApplicationService = clientsApplicationService;
        }

        /// <summary>
        /// Creates or updates client application
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The client application id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(ClientApplicationRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateClientApplicationServiceModel
                {
                    Id = request.Id,
                    CompanyName = request.CompanyName,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    ContactJobTitle = request.ContactJobTitle,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    CommunicationLanguage = request.CommunicationLanguage,
                    IsDeliveryAddressEqualBillingAddress = request.IsDeliveryAddressEqualBillingAddress,
                    BillingAddress = new ClientApplicationAddressServiceModel
                    {
                        Id = request.BillingAddress.Id,
                        FullName = request.BillingAddress.FullName,
                        PhoneNumber = request.BillingAddress.PhoneNumber,
                        Street = request.BillingAddress.Street,
                        Region = request.BillingAddress.Region,
                        PostalCode = request.BillingAddress.PostalCode,
                        City = request.BillingAddress.City,
                        Country = request.BillingAddress.Country
                    },
                    DeliveryAddress = new ClientApplicationAddressServiceModel
                    {
                        Id = request.DeliveryAddress.Id,
                        FullName = request.DeliveryAddress.FullName,
                        PhoneNumber = request.DeliveryAddress.PhoneNumber,
                        Street = request.DeliveryAddress.Street,
                        Region = request.DeliveryAddress.Region,
                        PostalCode = request.DeliveryAddress.PostalCode,
                        City = request.DeliveryAddress.City,
                        Country = request.DeliveryAddress.Country
                    },
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateClientApplicationModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var clientApplicationId = await _clientsApplicationService.UpdateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = clientApplicationId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateClientApplicationServiceModel
                {
                    CompanyName = request.CompanyName,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    ContactJobTitle = request.ContactJobTitle,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    CommunicationLanguage = request.CommunicationLanguage,
                    IsDeliveryAddressEqualBillingAddress = request.IsDeliveryAddressEqualBillingAddress,
                    BillingAddress = new ClientApplicationAddressServiceModel
                    { 
                        FullName = request.BillingAddress.FullName,
                        PhoneNumber = request.BillingAddress.PhoneNumber,
                        Street = request.BillingAddress.Street,
                        Region = request.BillingAddress.Region,
                        PostalCode = request.BillingAddress.PostalCode,
                        City = request.BillingAddress.City,
                        Country = request.BillingAddress.Country
                    },
                    DeliveryAddress = new ClientApplicationAddressServiceModel
                    {
                        FullName = request.DeliveryAddress.FullName,
                        PhoneNumber = request.DeliveryAddress.PhoneNumber,
                        Street = request.DeliveryAddress.Street,
                        Region = request.DeliveryAddress.Region,
                        PostalCode = request.DeliveryAddress.PostalCode,
                        City = request.DeliveryAddress.City,
                        Country = request.DeliveryAddress.Country
                    },
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateClientApplicationModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var clientApplicationId = await _clientsApplicationService.CreateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.Created, new { Id = clientApplicationId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Delete client application by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>OK.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new DeleteClientApplicationServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteClientApplicationModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _clientsApplicationService.DeleteAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Get information client application
        /// </summary>
        /// <param name="id">The client application id</param>
        /// <returns>The client application data.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<ClientApplicationResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetByEmail(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetClientApplicationServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetClientApplicationModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var clientApplication = await _clientsApplicationService.GetAsync(serviceModel);

                if (clientApplication is not null)
                {
                    var response = new ClientApplicationResponseModel
                    {
                        Id = clientApplication.Id,
                        CompanyName = clientApplication.CompanyName,
                        FirstName = clientApplication.FirstName,
                        LastName = clientApplication.LastName,
                        Email = clientApplication.Email,
                        PhoneNumber = clientApplication.PhoneNumber,
                        CommunicationLanguage = clientApplication.CommunicationLanguage,
                        ContactJobTitle = clientApplication.ContactJobTitle,
                        IsDeliveryAddressEqualBillingAddress = clientApplication.IsDeliveryAddressEqualBillingAddress,
                        BillingAddress = new ClientApplicationAddressResponseModel
                        {
                            Id = clientApplication.BillingAddress.Id,
                            FullName = clientApplication.BillingAddress.FullName,
                            PhoneNumber = clientApplication.BillingAddress.PhoneNumber,
                            Region = clientApplication.BillingAddress.Region,
                            Street = clientApplication.BillingAddress.Street,
                            PostalCode = clientApplication.BillingAddress.PostalCode,
                            City = clientApplication?.BillingAddress.City,
                            Country = clientApplication.BillingAddress.Country
                        },
                        LastModifiedDate = clientApplication.LastModifiedDate,
                        CreatedDate = clientApplication.CreatedDate
                    };

                    if (clientApplication.IsDeliveryAddressEqualBillingAddress is false)
                    {
                        response.DeliveryAddress = new ClientApplicationAddressResponseModel
                        {
                            Id = clientApplication.DeliveryAddress.Id,
                            FullName = clientApplication.DeliveryAddress.FullName,
                            PhoneNumber = clientApplication.DeliveryAddress.PhoneNumber,
                            Region = clientApplication.DeliveryAddress.Region,
                            Street = clientApplication.DeliveryAddress.Street,
                            PostalCode = clientApplication.DeliveryAddress.PostalCode,
                            City = clientApplication.DeliveryAddress.City,
                            Country = clientApplication.DeliveryAddress.Country
                        };
                    }

                    return StatusCode((int)HttpStatusCode.OK, response);
                }

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets list of clients applications.
        /// </summary>
        /// <param name="ids">The client applications ids.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of applications.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<ClientApplicationResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(string ids, string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var clientsApplicationIds = ids.ToEnumerableGuidIds();

            if (clientsApplicationIds.OrEmptyIfNull().Any())
            {
                var serviceModel = new GetClientsApplicationsByIdsServiceModel
                {
                    Ids = clientsApplicationIds,
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new GetClientsApplicationsByIdsModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var clientsApplications = await _clientsApplicationService.GetByIds(serviceModel);

                    if (clientsApplications is not null)
                    {
                        var response = new PagedResults<IEnumerable<ClientApplicationResponseModel>>(clientsApplications.Total, clientsApplications.PageSize)
                        {
                            Data = clientsApplications.Data.OrEmptyIfNull().Select(x => new ClientApplicationResponseModel
                            {
                                Id = x.Id,
                                CompanyName = x.CompanyName,
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                ContactJobTitle = x.ContactJobTitle,
                                PhoneNumber = x.PhoneNumber,
                                Email = x.Email,
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
                var serviceModel = new GetClientsApplicationsServiceModel
                {
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var clientsApplications = await _clientsApplicationService.GetAsync(serviceModel);

                if (clientsApplications is not null)
                {
                    var response = new PagedResults<IEnumerable<ClientApplicationResponseModel>>(clientsApplications.Total, clientsApplications.PageSize)
                    {
                        Data = clientsApplications.Data.OrEmptyIfNull().Select(x => new ClientApplicationResponseModel
                        {
                            Id = x.Id,
                            CompanyName = x.CompanyName,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            ContactJobTitle = x.ContactJobTitle,
                            PhoneNumber = x.PhoneNumber,
                            Email = x.Email,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        })
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }

                return StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}
