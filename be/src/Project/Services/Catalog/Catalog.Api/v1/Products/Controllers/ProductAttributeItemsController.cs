using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Foundation.Extensions.ExtensionMethods;
using System.Linq;
using System.Security.Claims;
using Foundation.Account.Definitions;
using Foundation.Extensions.Helpers;
using Catalog.Api.v1.Products.RequestModels;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Definitions;
using System.Globalization;
using Catalog.Api.v1.Products.ResultModels;
using Catalog.Api.Services.ProductAttributes;
using Catalog.Api.Validators.ProductAttributes;
using Catalog.Api.v1.Products.ResponseModels;
using Catalog.Api.ServicesModels.ProductAttributes;
using System.Collections.Generic;
using Foundation.GenericRepository.Paginations;

namespace Catalog.Api.v1.Products.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductAttributeItemsController : BaseApiController
    {
        private readonly IProductAttributesService productAttributesService;

        public ProductAttributeItemsController(IProductAttributesService productAttributesService)
        {
            this.productAttributesService = productAttributesService;
        }

        /// <summary>
        /// Returns product attribute items by product attribute id and search term. Returns all product attributes (paginated) if search term is empty.
        /// </summary>
        /// <param name="productAttributeId">The product attribute id.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns></returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<ProductAttributeItemResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        [AllowAnonymous]
        public async Task<IActionResult> Get(
            Guid? productAttributeId,
            string searchTerm,
            int pageIndex,
            int itemsPerPage,
            string orderBy)
        {
            var serviceModel = new GetProductAttributeItemsServiceModel
            {
                ProductAttributeId = productAttributeId,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                SearchTerm = searchTerm,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new GetProductAttributeItemsModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var productAttributes = await this.productAttributesService.GetProductAttributeItemsAsync(serviceModel);

                if (productAttributes != null)
                {
                    var response = new PagedResults<IEnumerable<ProductAttributeItemResponseModel>>(productAttributes.Total, productAttributes.PageSize)
                    {
                        Data = productAttributes.Data.OrEmptyIfNull().Select(x => new ProductAttributeItemResponseModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            ProductAttributeId = x.ProductAttributeId,
                            Order = x.Order,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        })
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Saves the product attribute item. Performs create if id is null and update otherwise.
        /// </summary>
        /// <param name="request">Product attribute item to save.</param>
        /// <returns>Product attribute item creation result.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ProductResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save([FromBody] ProductAttributeItemRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new CreateUpdateProductAttributeItemServiceModel
            {
                Id = request.Id,
                ProductAttributeId = request.ProductAttributeId,
                Name = request.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Language = CultureInfo.CurrentCulture.Name
            };

            if (request.Id.HasValue)
            {
                var validator = new UpdateProductAttributeItemModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var productAttributeItem = await this.productAttributesService.UpdateProductAttributeItemAsync(serviceModel);

                    if (productAttributeItem != null)
                    {
                        var response = new ProductAttributeItemResponseModel
                        {
                            Id = productAttributeItem.Id,
                            ProductAttributeId = productAttributeItem.ProductAttributeId,
                            Name = productAttributeItem.Name,
                            Order = productAttributeItem.Order,
                            LastModifiedDate = productAttributeItem.LastModifiedDate,
                            CreatedDate = productAttributeItem.CreatedDate
                        };

                        return this.StatusCode((int)HttpStatusCode.OK, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var validator = new CreateProductAttributeItemModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var productAttributeItem = await this.productAttributesService.CreateProductAttributeItemAsync(serviceModel);

                    if (productAttributeItem != null)
                    {
                        var response = new ProductAttributeItemResponseModel
                        {
                            Id = productAttributeItem.Id,
                            ProductAttributeId = productAttributeItem.ProductAttributeId,
                            Name = productAttributeItem.Name,
                            Order = productAttributeItem.Order,
                            LastModifiedDate = productAttributeItem.LastModifiedDate,
                            CreatedDate = productAttributeItem.CreatedDate
                        };

                        return this.StatusCode((int)HttpStatusCode.Created, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Returns a product attribute item by id.
        /// </summary>
        /// <param name="id">The product attribute item id.</param>
        /// <returns>The product attribute item.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetProductAttributeItemByIdServiceModel
            {
                Id = id.Value,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new GetProductAttributeItemByIdModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var productAttributeItem = await this.productAttributesService.GetProductAttributeItemByIdAsync(serviceModel);

                if (productAttributeItem != null)
                {
                    var response = new ProductAttributeItemResponseModel
                    {
                        Id = productAttributeItem.Id,
                        ProductAttributeId = productAttributeItem.ProductAttributeId,
                        Name = productAttributeItem.Name,
                        Order = productAttributeItem.Order,
                        LastModifiedDate = productAttributeItem.LastModifiedDate,
                        CreatedDate = productAttributeItem.CreatedDate
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.NoContent);
                }
            }

            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }


        /// <summary>
        /// Delete product attribute item by id.
        /// </summary>
        /// <param name="id">The product attribute item id.</param>
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

            var serviceModel = new DeleteProductAttributeItemServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteProductAttributeItemModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.productAttributesService.DeleteProductAttributeItemAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
