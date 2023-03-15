using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Inventory.Api.Services.Warehouses;
using Inventory.Api.ServicesModels.WarehouseServiceModels;
using Inventory.Api.v1.RequestModels;
using Inventory.Api.v1.ResponseModels;
using Inventory.Api.Validators.WarehouseValidators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inventory.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class WarehouseController : BaseApiController
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        /// <summary>
        /// Creates or updates warehouse (if warehouse id is set).
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The warehouse id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(WarehouseRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            if (request.Id.HasValue && request.Id != null)
            {
                var serviceModel = new UpdateWarehouseServiceModel
                {
                    Id = request.Id.Value,
                    Name = request.Name,
                    Location = request.Location,
                    Language = CultureInfo.CurrentCulture.Name,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateWarehouseModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var warehouse = await _warehouseService.UpdateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { warehouse.Id });
                }
                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateWarehouseServiceModel
                {
                    Name = request.Name,
                    Location = request.Location,
                    Language = CultureInfo.CurrentCulture.Name,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateWarehouseModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var warehouse = await _warehouseService.CreateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.Created, new { warehouse.Id });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Gets warehouse by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The warehouse.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetWarehouseServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetWarehouseModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var warehouse = await _warehouseService.GetAsync(serviceModel);

                if (warehouse != null)
                {
                    var response = new WarehouseResponseModel
                    {
                        Id = warehouse.Id,
                        Name = warehouse.Name,
                        Location = warehouse.Location,
                        LastModifiedDate = warehouse.LastModifiedDate,
                        CreatedDate = warehouse.CreatedDate
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.NoContent);
                }

            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets warehouse by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The warehouse.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("name/{name}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetWarehouseByName(string name)
        {
            var serviceModel = new GetWarehouseByNameServiceModel
            {
                Name = name,
            };

            var validator = new GetWarehouseByNameModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var warehouse = await _warehouseService.GetAsync(serviceModel);

                if (warehouse != null)
                {
                    var response = new WarehouseResponseModel
                    {
                        Id = warehouse.Id,
                        Name = warehouse.Name,
                        Location = warehouse.Location,
                        LastModifiedDate = warehouse.LastModifiedDate,
                        CreatedDate = warehouse.CreatedDate
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.NoContent);
                }

            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets list of warehouses.
        /// </summary>
        /// <param name="ids">The warehouse ids.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of warehouses.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(string ids, string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var warehouseIds = ids.ToEnumerableGuidIds();

            if (warehouseIds is not null)
            {
                var serviceModel = new GetWarehousesByIdsServiceModel
                {
                    Ids = warehouseIds,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    Language = CultureInfo.CurrentCulture.Name,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new GetWarehousesByIdsModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var warehouses = await _warehouseService.GetByIdsAsync(serviceModel);

                    if (warehouses is not null)
                    {
                        var response = new PagedResults<IEnumerable<WarehouseResponseModel>>(warehouses.Total, warehouses.PageSize)
                        {
                            Data = warehouses.Data.OrEmptyIfNull().Select(x => new WarehouseResponseModel
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Location = x.Location,
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
                var serviceModel = new GetWarehousesServiceModel
                {
                    Language = CultureInfo.CurrentCulture.Name,
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var warehouses = await _warehouseService.GetAsync(serviceModel);

                if (warehouses is not null)
                {
                    var response = new PagedResults<IEnumerable<WarehouseResponseModel>>(warehouses.Total, warehouses.PageSize)
                    {
                        Data = warehouses.Data.OrEmptyIfNull().Select(x => new WarehouseResponseModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Location = x.Location,
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
        /// Delete warehouse by id.
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
            var serviceModel = new DeleteWarehouseServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteWarehouseModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _warehouseService.DeleteAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }
            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
