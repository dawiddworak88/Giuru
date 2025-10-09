using Catalog.Api.Services.Products;
using Catalog.Api.ServicesModels.Products;
using Catalog.Api.v1.Products.RequestModels;
using Catalog.Api.v1.Products.ResponseModels;
using Catalog.Api.v1.Products.ResultModels;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.Search.Paginations;
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

namespace Catalog.Api.v1.Products.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductsSearchController : BaseApiController
    {
        private readonly IProductsService _productsService;

        public ProductsSearchController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        /// <summary>
        /// Gets list of products.
        /// </summary>
        /// <param name="ids">The optional list of product ids.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <param name="filters">The search filters.</param>
        /// <returns>The list of products.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Search(
            [FromBody] FiltersRequestModel filters,
            string ids,
            int? pageIndex,
            int? itemsPerPage,
            string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var productIds = ids.ToEnumerableGuidIds();

            if (productIds.OrEmptyIfNull().Any())
            {
                var serviceModel = new SearchProductsByIdsServiceModel
                {
                    Ids = productIds,
                    Filters = new SearchProductsFiltersServiceModel
                    {
                        Category = filters.Category,
                        Shape = filters.Shape,
                        Color = filters.Color,
                        Width = filters.Width,
                        Height = filters.Height,
                        Depth = filters.Depth
                    },
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                    IsSeller = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value == AccountConstants.Roles.Seller,
                    Language = CultureInfo.CurrentCulture.Name
                };

                var products = await _productsService.GetPagedResultsWithFiltersByIds(serviceModel);

                if (products is not null)
                {
                    var response = new PagedResultsWithFilters<IEnumerable<ProductResponseModel>>(products.Total, products.PageSize)
                    {
                        Data = products.Data.OrEmptyIfNull().Select(x => MapProductServiceModelToProductResponseModel(x)),
                        Filters = products.Filters
                    };

                    return Ok(response);
                }
            }
            else
            {
                var serviceModel = new SearchProductsServiceModel
                {
                    Filters = new SearchProductsFiltersServiceModel
                    {
                        Category = filters.Category,
                        Shape = filters.Shape,
                        Color = filters.Color,
                        Width = filters.Width,
                        Height = filters.Height,
                        Depth = filters.Depth
                    },
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                    IsSeller = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value == AccountConstants.Roles.Seller,
                    Language = CultureInfo.CurrentCulture.Name
                };

                var products = await _productsService.GetPagedResultsWithFilters(serviceModel);

                if (products is not null)
                {
                    var response = new PagedResultsWithFilters<IEnumerable<ProductResponseModel>>(products.Total, products.PageSize)
                    {
                        Data = products.Data.OrEmptyIfNull().Select(x => MapProductServiceModelToProductResponseModel(x)),
                        Filters = products.Filters
                    };

                    return Ok(response);
                }
            }

            return default;
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
