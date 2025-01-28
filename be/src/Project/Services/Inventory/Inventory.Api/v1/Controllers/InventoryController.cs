using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Inventory.Api.Services.InventoryItems;
using Inventory.Api.ServicesModels.InventoryServiceModels;
using Inventory.Api.v1.RequestModels;
using Inventory.Api.v1.ResponseModels;
using Inventory.Api.Validators.InventoryValidators;
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
    public class InventoryController : BaseApiController
    {
        private readonly IInventoryService _inventoriesService;

        public InventoryController(IInventoryService inventoriesService)
        {
            _inventoriesService = inventoriesService;
        }

        /// <summary>
        /// Gets a list of product inventories..
        /// </summary>
        /// <param name="ids">The list of inventory ids.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of product inventories.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(string ids, string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var inventoryIds = ids.ToEnumerableGuidIds();

            if (inventoryIds is not null)
            {
                var serviceModel = new GetInventoriesByIdsServiceModel
                {
                    Ids = inventoryIds,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    Language = CultureInfo.CurrentCulture.Name
                };

                var validator = new GetInventoriesByIdsModelValidator();
                var validationResult = validator.Validate(serviceModel);

                if (validationResult.IsValid)
                {
                    var inventories = _inventoriesService.GetByIds(serviceModel);

                    if (inventories is not null)
                    {
                        var response = new PagedResults<IEnumerable<InventoryResponseModel>>(inventories.Total, inventories.PageSize)
                        {
                            Data = inventories.Data.OrEmptyIfNull().Select(x => new InventoryResponseModel
                            {
                                Id = x.Id,
                                ProductId = x.ProductId,
                                ProductName = x.ProductName,
                                Sku = x.Sku,
                                Ean = x.Ean,
                                WarehouseId = x.WarehouseId,
                                WarehouseName = x.WarehouseName,
                                Quantity = x.Quantity,
                                AvailableQuantity = x.AvailableQuantity,
                                RestockableInDays = x.RestockableInDays,
                                ExpectedDelivery = x.ExpectedDelivery,
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
                var serviceModel = new GetInventoriesServiceModel
                {
                    Language = CultureInfo.CurrentCulture.Name,
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var inventories = _inventoriesService.Get(serviceModel);

                if (inventories is not null)
                {
                    var response = new PagedResults<IEnumerable<InventoryResponseModel>>(inventories.Total, inventories.PageSize)
                    {
                        Data = inventories.Data.OrEmptyIfNull().Select(x => new InventoryResponseModel
                        {
                            Id = x.Id,
                            WarehouseId = x.WarehouseId,
                            WarehouseName = x.WarehouseName,
                            ProductId = x.ProductId,
                            ProductName = x.ProductName,
                            Sku = x.Sku,
                            Quantity = x.Quantity,
                            Ean = x.Ean,
                            AvailableQuantity = x.AvailableQuantity,
                            RestockableInDays = x.RestockableInDays,
                            ExpectedDelivery = x.ExpectedDelivery,
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
        /// Gets a list of products available on stock.
        /// </summary>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The list of products available on stock..</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [Route("availableproducts")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult GetAvailableProductsInventories(
            int? pageIndex, 
            int? itemsPerPage)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            
            var serviceModel = new GetInventoriesServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var inventories = _inventoriesService.GetAvailableProductsInventories(serviceModel);

            if (inventories is not null)
            {
                var response = new PagedResults<IEnumerable<InventorySumResponseModel>>(inventories.Total, inventories.PageSize)
                {
                    Data = inventories.Data.OrEmptyIfNull().Select(inventoryProduct => new InventorySumResponseModel
                    {
                        ProductId = inventoryProduct.ProductId,
                        AvailableQuantity = inventoryProduct.AvailableQuantity,
                        Quantity = inventoryProduct.Quantity,
                        ProductName = inventoryProduct.ProductName,
                        ProductSku = inventoryProduct.ProductSku,
                        Ean = inventoryProduct.ProductEan,
                        RestockableInDays = inventoryProduct.RestockableInDays,
                        ExpectedDelivery = inventoryProduct.ExpectedDelivery
                    })
                };

                return this.StatusCode((int)HttpStatusCode.OK, response);
            }

            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates or updates inventory (if inventory skus are set).
        /// </summary>
        /// <param name="request">The list of inventory items.</param>
        /// <returns>The successfully saved inventory ids.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [Route("productinventories")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> SaveProductInventories(SaveInventoriesBySkusRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new UpdateProductsInventoryServiceModel
            {
                InventoryItems = request.InventoryItems.OrEmptyIfNull().Select(x => new UpdateProductInventoryServiceModel
                {
                    WarehouseId = x.WarehouseId,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductSku = x.ProductSku,
                    ProductEan = x.Ean,
                    Quantity = x.Quantity,
                    AvailableQuantity = x.AvailableQuantity,
                    ExpectedDelivery = x.ExpectedDelivery,
                    RestockableInDays = x.RestockableInDays
                }),
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new SaveInventoryItemsByProductSkusModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _inventoriesService.SyncProductsInventories(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates or updates an inventory.
        /// </summary>
        /// <param name="request">The inventory details to save.</param>
        /// <returns>The inventory id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(InventoryRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue && request.Id is not null)
            {
                var serviceModel = new UpdateInventoryServiceModel
                {
                    Id = request.Id.Value,
                    WarehouseId = request.WarehouseId.Value,
                    ProductId = request.ProductId.Value,
                    ProductName = request.ProductName,
                    ProductSku = request.ProductSku,
                    Quantity = request.Quantity,
                    ProductEan = request.Ean,
                    AvailableQuantity = request.AvailableQuantity,
                    RestockableInDays = request.RestockableInDays,
                    ExpectedDelivery = request.ExpectedDelivery,
                    Language = CultureInfo.CurrentCulture.Name,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateInventoryModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var inventoryProduct = await _inventoriesService.UpdateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { inventoryProduct.Id });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateInventoryServiceModel
                {
                    WarehouseId = request.WarehouseId.Value,
                    ProductId = request.ProductId.Value,
                    ProductName = request.ProductName,
                    ProductSku = request.ProductSku,
                    Quantity = request.Quantity,
                    ProductEan = request.Ean,
                    AvailableQuantity = request.AvailableQuantity,
                    RestockableInDays = request.RestockableInDays,
                    ExpectedDelivery = request.ExpectedDelivery,
                    Language = CultureInfo.CurrentCulture.Name,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateInventoryModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var inventoryProduct = await _inventoriesService.CreateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.Created, new { inventoryProduct.Id });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Gets a product inventory by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The inventory.</returns>
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
            var serviceModel = new GetInventoryServiceModel
            {
                Id = id.Value,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetInventoryModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var inventoryProduct = await _inventoriesService.GetAsync(serviceModel);

                if (inventoryProduct is not null)
                {
                    var response = new InventoryResponseModel
                    {
                        Id = inventoryProduct.Id,
                        ProductId = inventoryProduct.ProductId,
                        ProductName = inventoryProduct.ProductName,
                        Sku = inventoryProduct.Sku,
                        WarehouseId = inventoryProduct.WarehouseId.Value,
                        WarehouseName = inventoryProduct.WarehouseName,
                        Quantity = inventoryProduct.Quantity,
                        Ean = inventoryProduct.Ean,
                        AvailableQuantity = inventoryProduct.AvailableQuantity,
                        RestockableInDays = inventoryProduct.RestockableInDays,
                        ExpectedDelivery = inventoryProduct.ExpectedDelivery,
                        LastModifiedDate = inventoryProduct.LastModifiedDate,
                        CreatedDate = inventoryProduct.CreatedDate,
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }

            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets a product inventory by product id.
        /// </summary>
        /// <param name="id">The product id.</param>
        /// <returns>The product inventory.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("product/{id}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetInventoryByProductId(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var serviceModel = new GetInventoryByProductIdServiceModel
            {
                ProductId = id.Value,
            };

            var validator = new GetProductByIdModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var inventoryProduct = await _inventoriesService.GetInventoryByProductId(serviceModel);

                if (inventoryProduct is not null)
                {
                    var response = new InventorySumResponseModel
                    {
                        ProductId = inventoryProduct.ProductId,
                        AvailableQuantity = inventoryProduct.AvailableQuantity,
                        Quantity = inventoryProduct.Quantity,
                        Ean = inventoryProduct.ProductEan,
                        ProductName = inventoryProduct.ProductName,
                        ProductSku = inventoryProduct.ProductSku,
                        RestockableInDays = inventoryProduct.RestockableInDays,
                        ExpectedDelivery = inventoryProduct.ExpectedDelivery,
                        Details = inventoryProduct.Details.Select(item => new InventoryDetailsResponseModel 
                        { 
                            Id = item.Id,
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            Ean = item.Ean,
                            AvailableQuantity = item.AvailableQuantity,
                            ExpectedDelivery = item.ExpectedDelivery,
                            ProductSku = item.Sku,
                            WarehouseId = item.WarehouseId,
                            WarehouseName = item.WarehouseName,
                            RestockableInDays = item.RestockableInDays,
                            LastModifiedDate = item.LastModifiedDate,
                            CreatedDate = item.CreatedDate
                        })
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
        /// Gets a products inventories by products ids.
        /// </summary>
        /// <param name="ids">The products ids.</param>
        /// <returns>The list of products inventories.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("product/ids/{ids}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetInventoriesByProductsIds(string ids)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetInventoriesByProductsIdsServiceModel
            {
                Ids = ids.ToEnumerableGuidIds(),
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new GetInventoriesByProductsIdsModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var inventories = _inventoriesService.GetInventoriesByProductsIds(serviceModel);

                var response = inventories.Select(x => new InventorySumResponseModel
                {
                    ProductId = x.ProductId,
                    AvailableQuantity = x.AvailableQuantity,
                    Quantity = x.Quantity,
                    Ean = x.ProductEan,
                    ProductName = x.ProductName,
                    ProductSku = x.ProductSku,
                    RestockableInDays = x.RestockableInDays,
                    ExpectedDelivery = x.ExpectedDelivery
                });

                return StatusCode((int)HttpStatusCode.OK, response);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets a product inventory by product sku.
        /// </summary>
        /// <param name="sku">The product sku.</param>
        /// <returns>The product inventory.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("product/sku/{sku}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetInventoryByProductSku(string sku)
        {
            var serviceModel = new GetInventoryByProductSkuServiceModel
            {
                ProductSku = sku,
            };

            var validator = new GetProductBySkuModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var inventoryProduct = await _inventoriesService.GetInventoryByProductSku(serviceModel);

                if (inventoryProduct is not null)
                {
                    var response = new InventorySumResponseModel
                    {
                        ProductId = inventoryProduct.ProductId,
                        AvailableQuantity = inventoryProduct.AvailableQuantity,
                        Quantity = inventoryProduct.Quantity,
                        Ean = inventoryProduct.ProductEan,
                        ProductName = inventoryProduct.ProductName,
                        ProductSku = inventoryProduct.ProductSku,
                        RestockableInDays = inventoryProduct.RestockableInDays,
                        ExpectedDelivery = inventoryProduct.ExpectedDelivery,
                        Details = inventoryProduct.Details.Select(item => new InventoryDetailsResponseModel
                        {
                            Id = item.Id,
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            Ean = item.Ean,
                            AvailableQuantity = item.AvailableQuantity,
                            ExpectedDelivery = item.ExpectedDelivery,
                            ProductSku = item.Sku,
                            WarehouseId = item.WarehouseId,
                            WarehouseName = item.WarehouseName,
                            RestockableInDays = item.RestockableInDays,
                            LastModifiedDate = item.LastModifiedDate,
                            CreatedDate = item.CreatedDate
                        })
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
        /// Deletes a product inventory by id.
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
            var serviceModel = new DeleteInventoryServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteInventoryModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _inventoriesService.DeleteAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
