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
using Inventory.Api.v1.ResponseModels;
using Inventory.Api.Validators.OutletValidators;
using Inventory.Api.ServicesModels.OutletServiceModels;
using Inventory.Api.v1.RequestModels;
using Inventory.Api.Services.OutletItems;

namespace Outlet.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class OutletController : BaseApiController
    {
        private readonly IOutletService outletsService;

        public OutletController(IOutletService outletsService)
        {
            this.outletsService = outletsService;
        }

        /// <summary>
        /// Gets a list of outlet products.
        /// </summary>
        /// <param name="ids">The list of outlet ids.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of product outlet items.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(string ids, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var outletIds = ids.ToEnumerableGuidIds();
            if (outletIds != null)
            {
                var serviceModel = new GetOutletsByIdsServiceModel
                {
                    Ids = outletIds,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    Language = CultureInfo.CurrentCulture.Name
                };

                var validator = new GetOutletsByIdsModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var outlets = await this.outletsService.GetByIdsAsync(serviceModel);
                    if (outlets != null)
                    {
                        var response = new PagedResults<IEnumerable<OutletResponseModel>>(outlets.Total, outlets.PageSize)
                        {
                            Data = outlets.Data.OrEmptyIfNull().Select(x => new OutletResponseModel
                            {
                                Id = x.Id,
                                ProductId = x.ProductId,
                                ProductName = x.ProductName,
                                ProductSku = x.ProductSku,
                                WarehouseId = x.WarehouseId,
                                WarehouseName = x.WarehouseName,
                                Quantity = x.Quantity,
                                Title = x.Title,
                                Description = x.Description,
                                Ean = x.ProductEan,
                                AvailableQuantity = x.AvailableQuantity,
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
                var serviceModel = new GetOutletsServiceModel
                {
                    Language = CultureInfo.CurrentCulture.Name,
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var outlets = await this.outletsService.GetAsync(serviceModel);
                if (outlets != null)
                {
                    var response = new PagedResults<IEnumerable<OutletResponseModel>>(outlets.Total, outlets.PageSize)
                    {
                        Data = outlets.Data.OrEmptyIfNull().Select(x => new OutletResponseModel
                        {
                            Id = x.Id,
                            WarehouseId = x.WarehouseId,
                            WarehouseName = x.WarehouseName,
                            ProductId = x.ProductId,
                            ProductName = x.ProductName,
                            ProductSku = x.ProductSku,
                            Quantity = x.Quantity,
                            Title = x.Title,
                            Description = x.Description,
                            Ean = x.ProductEan,
                            AvailableQuantity = x.AvailableQuantity,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        })
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }

                throw new CustomException("", (int)HttpStatusCode.UnprocessableEntity);
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
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetAvailableOutletProducts(int pageIndex, int itemsPerPage)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetOutletsServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var outlets = await this.outletsService.GetAvailableProductsOutletsAsync(serviceModel);

            if (outlets != null)
            {
                var response = new PagedResults<IEnumerable<OutletSumResponseModel>>(outlets.Total, outlets.PageSize)
                {
                    Data = outlets.Data.OrEmptyIfNull().Select(outletProduct => new OutletSumResponseModel
                    {
                        ProductId = outletProduct.ProductId,
                        AvailableQuantity = outletProduct.AvailableQuantity,
                        Quantity = outletProduct.Quantity,
                        ProductName = outletProduct.ProductName,
                        ProductSku = outletProduct.ProductSku,
                        Title = outletProduct.Title,
                        Description = outletProduct.Description,
                        Ean = outletProduct.ProductEan
                    })
                };

                return this.StatusCode((int)HttpStatusCode.OK, response);
            }

            throw new CustomException("", (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates or updates outlet (if outlet skus are set).
        /// </summary>
        /// <param name="request">The list of outlet items.</param>
        /// <returns>The successfully saved outlet ids.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [Route("outletitems")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> SaveProductOutletItems(SaveOutletsBySkusRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new UpdateOutletProductsServiceModel
            {
                OutletItems = request.OutletItems.OrEmptyIfNull().Select(x => new UpdateOutletProductServiceModel
                {
                    WarehouseId = x.WarehouseId,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductSku = x.ProductSku,
                    ProductEan = x.Ean,
                    Quantity = x.Quantity,
                    AvailableQuantity = x.AvailableQuantity

                }),
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new SaveOutletItemsByProductSkusModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.outletsService.SyncProductsOutlet(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates or updates an outlet.
        /// </summary>
        /// <param name="request">The outlet details to save.</param>
        /// <returns>The outlet id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(OutletRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue && request.Id != null)
            {
                var serviceModel = new UpdateOutletServiceModel
                {
                    Id = request.Id.Value,
                    WarehouseId = request.WarehouseId.Value,
                    ProductId = request.ProductId.Value,
                    ProductName = request.ProductName,
                    ProductSku = request.ProductSku,
                    Quantity = request.Quantity,
                    AvailableQuantity = request.AvailableQuantity,
                    Title = request.Title,
                    Description = request.Description,
                    ProductEan = request.Ean,
                    Language = CultureInfo.CurrentCulture.Name,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateOutletModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var outletProduct = await this.outletsService.UpdateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { outletProduct.Id });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateOutletServiceModel
                {
                    WarehouseId = request.WarehouseId.Value,
                    ProductId = request.ProductId.Value,
                    ProductName = request.ProductName,
                    ProductSku = request.ProductSku,
                    Quantity = request.Quantity,
                    AvailableQuantity = request.AvailableQuantity,
                    Title = request.Title,
                    Description = request.Description,
                    ProductEan = request.Ean,
                    Language = CultureInfo.CurrentCulture.Name,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateOutletModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var outletProduct = await this.outletsService.CreateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.Created, new { outletProduct.Id });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Gets an outlet item by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The outlet.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var serviceModel = new GetOutletServiceModel
            {
                Id = id.Value,
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetOutletModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var outletProduct = await this.outletsService.GetAsync(serviceModel);

                if (outletProduct != null)
                {
                    var response = new OutletResponseModel
                    {
                        Id = outletProduct.Id,
                        ProductId = outletProduct.ProductId,
                        ProductName = outletProduct.ProductName,
                        ProductSku = outletProduct.ProductSku,
                        WarehouseId = outletProduct.WarehouseId.Value,
                        WarehouseName = outletProduct.WarehouseName,
                        Quantity = outletProduct.Quantity,
                        Title = outletProduct.Title,
                        Description = outletProduct.Description,
                        Ean = outletProduct.ProductEan,
                        AvailableQuantity = outletProduct.AvailableQuantity,
                        LastModifiedDate = outletProduct.LastModifiedDate,
                        CreatedDate = outletProduct.CreatedDate,
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.NotFound);
                }

            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets an outlet item by product id.
        /// </summary>
        /// <param name="id">The product id.</param>
        /// <returns>The product outlet.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("product/{id}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetOutletByProductId(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var serviceModel = new GetOutletByProductIdServiceModel
            {
                ProductId = id,
            };

            var validator = new GetProductByIdModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var outletProduct = await this.outletsService.GetOutletByProductId(serviceModel);

                if (outletProduct != null)
                {
                    var response = new OutletSumResponseModel
                    {
                        ProductId = outletProduct.ProductId,
                        AvailableQuantity = outletProduct.AvailableQuantity,
                        Quantity = outletProduct.Quantity,
                        ProductName = outletProduct.ProductName,
                        ProductSku = outletProduct.ProductSku,
                        Ean = outletProduct.ProductEan,
                        Title = outletProduct.Title,
                        Description = outletProduct.Description,
                        Details = outletProduct.Details.Select(item => new OutletDetailsResponseModel
                        {
                            Id = item.Id,
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            AvailableQuantity = item.AvailableQuantity,
                            ProductSku = item.ProductSku,
                            WarehouseId = item.WarehouseId,
                            WarehouseName = item.WarehouseName,
                            Title = item.Title,
                            Description = item.Description,
                            Ean = item.ProductEan,
                            LastModifiedDate = item.LastModifiedDate,
                            CreatedDate = item.CreatedDate
                        })
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.NotFound);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }


        /// <summary>
        /// Gets an outlet item by product sku.
        /// </summary>
        /// <param name="sku">The product sku.</param>
        /// <returns>The product outlet.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("product/sku/{sku}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetOutletByProductSku(string sku)
        {
            var serviceModel = new GetOutletByProductSkuServiceModel
            {
                ProductSku = sku
            };

            var validator = new GetProductBySkuModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var outletProduct = await this.outletsService.GetOutletByProductSku(serviceModel);

                if (outletProduct != null)
                {
                    var response = new OutletSumResponseModel
                    {
                        ProductId = outletProduct.ProductId,
                        AvailableQuantity = outletProduct.AvailableQuantity,
                        Quantity = outletProduct.Quantity,
                        ProductName = outletProduct.ProductName,
                        ProductSku = outletProduct.ProductSku,
                        Title = outletProduct.Title,
                        Description = outletProduct.Description,
                        Details = outletProduct.Details.Select(item => new OutletDetailsResponseModel
                        {
                            Id = item.Id,
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            AvailableQuantity = item.AvailableQuantity,
                            ProductSku = item.ProductSku,
                            WarehouseId = item.WarehouseId,
                            WarehouseName = item.WarehouseName,
                            Title = item.Title,
                            Description = item.Description,
                            Ean = item.ProductEan,
                            LastModifiedDate = item.LastModifiedDate,
                            CreatedDate = item.CreatedDate
                        })
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.NotFound);
                }

            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }


        /// <summary>
        /// Deletes an outlet item by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>OK.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var serviceModel = new DeleteOutletServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteOutletModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.outletsService.DeleteAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
