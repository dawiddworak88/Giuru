using Catalog.Api.ServicesModels.Products;
using Catalog.Api.Services.Products;
using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Catalog.Api.Validators.Products;
using Foundation.Extensions.ExtensionMethods;
using System.Linq;
using System.Security.Claims;
using Foundation.Account.Definitions;
using Foundation.Extensions.Helpers;
using Catalog.Api.v1.Products.RequestModels;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Definitions;
using System.Globalization;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using Catalog.Api.v1.Products.ResultModels;
using Catalog.Api.v1.Products.ResponseModels;

namespace Catalog.Api.v1.Products.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductsController : BaseApiController
    {
        private readonly IProductsService _productService;

        public ProductsController(IProductsService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get list of product files.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <param name="id">The product id.</param>
        /// <returns>The list of product files.</returns>
        [HttpGet("files/{id}"), MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<ProductFileResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Files(Guid? id, string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetProductFilesServiceModel
            {
                Id = id,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetProductFilesModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var productFiles = await _productService.GetProductFiles(serviceModel);

                if (productFiles is not null)
                {
                    var response = new PagedResults<IEnumerable<ProductFileResponseModel>>(productFiles.Total, productFiles.PageSize)
                    {
                        Data = productFiles.Data.OrEmptyIfNull().Select(x => new ProductFileResponseModel
                        {
                            Id = x.Id,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        })
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets list of products by Skus.
        /// </summary>
        /// <param name="skus">The list of skus.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of products.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [Route("skus")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetBySkus(
            string skus,
            int? pageIndex, 
            int? itemsPerPage, 
            string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var productSkus = skus.ToEnumerableString();

            if (productSkus is not null)
            {
                var serviceModel = new GetProductsBySkusServiceModel
                {
                    Skus = productSkus,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                    IsSeller = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value == AccountConstants.Roles.Seller,
                    Language = CultureInfo.CurrentCulture.Name
                };

                var validator = new GetProductsBySkusModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var products = await _productService.GetBySkusAsync(serviceModel);

                    if (products is not null)
                    {
                        var response = new PagedResults<IEnumerable<ProductResponseModel>>(products.Total, products.PageSize)
                        {
                            Data = products.Data.OrEmptyIfNull().Select(x => MapProductServiceModelToProductResponseModel(x))
                        };

                        return StatusCode((int)HttpStatusCode.OK, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }

            return default;
        }

        /// <summary>
        /// Returns products by search term. Returns all products (paginated) if search term is empty.
        /// </summary>
        /// <param name="ids">The list of product ids.</param>
        /// <param name="categoryId">The category id.</param>
        /// <param name="hasPrimaryProduct">Set to null to get all products including product variants. Set to false to get primary products only. Set to true to get product variants only.</param>
        /// <param name="isNew">Set to null to get all products. Set to true to get new products only.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns></returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<ProductResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        [AllowAnonymous]
        public async Task<IActionResult> Get(
            string ids, 
            Guid? categoryId, 
            bool? hasPrimaryProduct,
            bool? isNew,
            string searchTerm, 
            int? pageIndex, 
            int? itemsPerPage,
            string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var productIds = ids.ToEnumerableGuidIds();

            if (productIds is not null)
            {
                var serviceModel = new GetProductsByIdsServiceModel
                {
                    Ids = productIds,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    Language = CultureInfo.CurrentCulture.Name,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                    IsSeller = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value == AccountConstants.Roles.Seller
                };

                var validator = new GetProductsByIdsModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var products = await _productService.GetByIdsAsync(serviceModel);

                    if (products is not null)
                    {
                        var response = new PagedResults<IEnumerable<ProductResponseModel>>(products.Total, products.PageSize)
                        { 
                            Data = products.Data.OrEmptyIfNull().Select(x => MapProductServiceModelToProductResponseModel(x))
                        };

                        return StatusCode((int)HttpStatusCode.OK, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new GetProductsServiceModel
                {
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    SearchTerm = searchTerm,
                    CategoryId = categoryId,
                    OrderBy = orderBy,
                    Language = CultureInfo.CurrentCulture.Name,
                    HasPrimaryProduct = hasPrimaryProduct,
                    IsNew = isNew,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                    IsSeller = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value == AccountConstants.Roles.Seller
                };

                var products = await _productService.GetAsync(serviceModel);

                if (products is not null)
                {
                    var response = new PagedResults<IEnumerable<ProductResponseModel>>(products.Total, products.PageSize)
                    {
                        Data = products.Data.OrEmptyIfNull().Select(x => MapProductServiceModelToProductResponseModel(x))
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }

                return StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Saves the product. Performs create if id is null and update otherwise.
        /// </summary>
        /// <param name="request">Product to save.</param>
        /// <returns>Product creation result.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ProductResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save([FromBody] ProductRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new CreateUpdateProductModel
            {
                Id = request.Id,
                PrimaryProductId = request.PrimaryProductId,
                IsNew = request.IsNew,
                IsPublished = request.IsPublished,
                CategoryId = request.CategoryId,
                IsProtected = request.IsProtected,
                Videos = request.Videos,
                Files = request.Files,
                Images = request.Images,
                Sku = request.Sku,
                Name = request.Name,
                Description = request.Description,
                FormData = request.FormData,
                Ean = request.Ean,
                FulfillmentTime = request.FulfillmentTime,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Language = CultureInfo.CurrentCulture.Name
            };

            if (request.Id.HasValue)
            {
                var validator = new UpdateProductModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var productId = await _productService.UpdateAsync(serviceModel);

                    if (productId != null)
                    {
                        return StatusCode((int)HttpStatusCode.OK, new { Id = productId });
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var validator = new CreateProductModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var productId = await _productService.CreateAsync(serviceModel);

                    if (productId != null)
                    {
                        return StatusCode((int)HttpStatusCode.Created, new { Id = productId });
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Returns a product by id.
        /// </summary>
        /// <param name="id">The product id.</param>
        /// <returns>The product.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetProductByIdServiceModel
            {
                Id = id.Value,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                IsSeller = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value == AccountConstants.Roles.Seller,
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new GetProductByIdModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var product = await _productService.GetByIdAsync(serviceModel);

                if (product != null)
                {
                    return StatusCode((int)HttpStatusCode.OK, MapProductServiceModelToProductResponseModel(product));
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.NoContent);
                }
            }

            return StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Returns a product by sku.
        /// </summary>
        /// <param name="sku">The product sku.</param>
        /// <returns>The product.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("sku/{sku}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [AllowAnonymous]
        public async Task<IActionResult> GetBySku(string sku)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetProductBySkuServiceModel
            {
                Sku = sku,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                IsSeller = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value == AccountConstants.Roles.Seller,
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new GetProductBySkuModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var product = await _productService.GetBySkuAsync(serviceModel);

                if (product != null)
                {
                    return StatusCode((int)HttpStatusCode.OK, MapProductServiceModelToProductResponseModel(product));
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.NoContent);
                }
            }

            return StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Delete product by id.
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

            var serviceModel = new DeleteProductServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteProductModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _productService.DeleteAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        private static ProductResponseModel MapProductServiceModelToProductResponseModel(ProductServiceModel product)
        {
            return new ProductResponseModel
            {
                Id = product.Id,
                BrandName = product.BrandName,
                CategoryId = product.CategoryId,
                CategoryName = product.CategoryName,
                Description = product.Description,
                Files = product.Files,
                Images = product.Images,
                FormData = product.FormData,
                IsNew = product.IsNew,
                IsProtected = product.IsProtected,
                IsPublished = product.IsPublished,
                Name = product.Name,
                PrimaryProductId = product.PrimaryProductId,
                PrimaryProductSku = product.PrimaryProductSku,
                ProductVariants = product.ProductVariants,
                SellerId = product.SellerId,
                Sku = product.Sku,
                Videos = product.Videos,
                Ean = product.Ean,
                FulfillmentTime = product.FulfillmentTime,
                StockAvailableQuantity = product.StockAvailableQuantity,
                OutletAvailableQuantity = product.OutletAvailableQuantity,
                ProductAttributes = product.ProductAttributes.OrEmptyIfNull().Select(x => new ProductAttributeValuesResponseModel
                {
                    Key = x.Key,
                    Name = x.Name,
                    Values = x.Values.OrEmptyIfNull().Select(x => x)
                }),
                LastModifiedDate = product.LastModifiedDate,
                CreatedDate = product.CreatedDate
            };
        }
    }
}
