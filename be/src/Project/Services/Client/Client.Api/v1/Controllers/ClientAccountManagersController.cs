using Client.Api.Services.Managers;
using Client.Api.ServicesModels.Managers;
using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Client.Api.Validators.Managers;
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
    public class ClientAccountManagersController : BaseApiController
    {
        private readonly IClientAccountManagersService clientManagersService;

        public ClientAccountManagersController(
            IClientAccountManagersService clientManagersService)
        {
            this.clientManagersService = clientManagersService;
        }

        /// <summary>
        /// Delete manager by id.
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
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var serviceModel = new DeleteClientAccountManagerServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteClientAccountManagerModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);
            if (validationResult.IsValid)
            {
                await this.clientManagersService.DeleteAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets manager by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The manager.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ClientAccountManagerResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            
            var serviceModel = new GetClientAccountManagerServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetClientAccountManagerModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var manager = await this.clientManagersService.GetAsync(serviceModel);

                if (manager is not null)
                {
                    var response = new ClientAccountManagerResponseModel
                    {
                        Id = manager.Id,
                        FirstName = manager.FirstName,
                        LastName = manager.LastName,
                        Email = manager.Email,
                        PhoneNumber = manager.PhoneNumber,
                        LastModifiedDate = manager.LastModifiedDate,
                        CreatedDate = manager.CreatedDate
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }

                return this.StatusCode((int)HttpStatusCode.NoContent);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets list of managers.
        /// </summary>
        /// <param name="ids">The managers ids.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of managers.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<ClientAccountManagerResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(string ids, string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var clientManagerIds = ids.ToEnumerableGuidIds();

            if (clientManagerIds.OrEmptyIfNull().Any())
            {
                var serviceModel = new GetClientAccountManagersByIdsServiceModel
                {
                    Ids = clientManagerIds,
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new GetClientAccountManagersByIdsModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var managers = await this.clientManagersService.GetByIdsAsync(serviceModel);

                    if (managers is not null)
                    {
                        var response = new PagedResults<IEnumerable<ClientAccountManagerResponseModel>>(managers.Total, managers.PageSize)
                        {
                            Data = managers.Data.OrEmptyIfNull().Select(x => new ClientAccountManagerResponseModel
                            {
                                Id = x.Id,
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                LastModifiedDate = x.LastModifiedDate,
                                CreatedDate = x.CreatedDate
                            })
                        };

                        return this.StatusCode((int)HttpStatusCode.OK, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new GetClientAccountManagersServiceModel
                {
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var managers = await this.clientManagersService.GetAsync(serviceModel);

                if (managers is not null)
                {
                    var response = new PagedResults<IEnumerable<ClientAccountManagerResponseModel>>(managers.Total, managers.PageSize)
                    {
                        Data = managers.Data.OrEmptyIfNull().Select(x => new ClientAccountManagerResponseModel
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            Email = x.Email,
                            PhoneNumber = x.PhoneNumber,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        })
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }

                return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Creates or updates manager (if manager id is set).
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The manager id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(ClientAccountManagerRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateClientAccountManagerServiceModel
                {
                    Id = request.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateClientAccountManagerModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var managerId = await this.clientManagersService.UpdateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { Id = managerId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateClientAccountManagerServiceModel
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateClientAccountManagerModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var managerId = await this.clientManagersService.CreateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { Id = managerId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}
