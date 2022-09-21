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
    public class ProductAttributesController : BaseApiController
    {
        private readonly IProductAttributesService productAttributesService;

        public ProductAttributesController(IProductAttributesService productAttributesService)
        {
            this.productAttributesService = productAttributesService;
        }

        /// <summary>
        /// Returns product attributes by search term. Returns all products (paginated) if search term is empty.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns></returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<ProductAttributeResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        [AllowAnonymous]
        public async Task<IActionResult> Get(
            string searchTerm,
            int pageIndex,
            int itemsPerPage,
            string orderBy)
        {
            var serviceModel = new GetProductAttributesServiceModel
            {
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                SearchTerm = searchTerm,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new GetProductAttributesModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var productAttributes = await this.productAttributesService.GetAsync(serviceModel);

                if (productAttributes != null)
                {
                    var response = new PagedResults<IEnumerable<ProductAttributeResponseModel>>(productAttributes.Total, productAttributes.PageSize)
                    {
                        Data = productAttributes.Data.OrEmptyIfNull().Select(x => new ProductAttributeResponseModel
                        {
                            Id = x.Id,
                            Name = x.Name,
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
        /// Saves the product attribute. Performs create if id is null and update otherwise.
        /// </summary>
        /// <param name="request">Product attribute to save.</param>
        /// <returns>Product attribute creation result.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ProductResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save([FromBody] ProductAttributeRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new CreateUpdateProductAttributeServiceModel
            {
                Id = request.Id,
                Name = request.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Language = CultureInfo.CurrentCulture.Name
            };

            if (request.Id.HasValue)
            {
                var validator = new UpdateProductAttributeModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var productAttribute = await this.productAttributesService.UpdateProductAttributeAsync(serviceModel);

                    if (productAttribute != null)
                    {
                        var response = new ProductAttributeResponseModel
                        {
                            Id = productAttribute.Id,
                            Name = productAttribute.Name,
                            Order = productAttribute.Order,
                            Items = productAttribute.ProductAttributeItems.OrEmptyIfNull().Select(x => new ProductAttributeItemResponseModel 
                            {
                                Id = x.Id,
                                ProductAttributeId = x.ProductAttributeId,
                                Name = x.Name,
                                Order = x.Order,
                                LastModifiedDate = x.LastModifiedDate,
                                CreatedDate = x.CreatedDate
                            }),
                            LastModifiedDate = productAttribute.LastModifiedDate,
                            CreatedDate = productAttribute.CreatedDate
                        };

                        return this.StatusCode((int)HttpStatusCode.OK, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var validator = new CreateProductAttributeModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var productAttribute = await this.productAttributesService.CreateProductAttributeAsync(serviceModel);

                    if (productAttribute != null)
                    {
                        var response = new ProductAttributeResponseModel
                        {
                            Id = productAttribute.Id,
                            Name = productAttribute.Name,
                            Order = productAttribute.Order,
                            Items = productAttribute.ProductAttributeItems.OrEmptyIfNull().Select(x => new ProductAttributeItemResponseModel
                            {
                                Id = x.Id,
                                ProductAttributeId = x.ProductAttributeId,
                                Name = x.Name,
                                Order = x.Order,
                                LastModifiedDate = x.LastModifiedDate,
                                CreatedDate = x.CreatedDate
                            }),
                            LastModifiedDate = productAttribute.LastModifiedDate,
                            CreatedDate = productAttribute.CreatedDate
                        };

                        return this.StatusCode((int)HttpStatusCode.Created, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Returns a product attribute by id.
        /// </summary>
        /// <param name="id">The product attribute id.</param>
        /// <returns>The product attribute.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetProductAttributeByIdServiceModel
            {
                Id = id.Value,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new GetProductAttributeByIdModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var productAttribute = await this.productAttributesService.GetProductAttributeByIdAsync(serviceModel);

                if (productAttribute != null)
                {
                    var response = new ProductAttributeResponseModel
                    {
                        Id = productAttribute.Id,
                        Name = productAttribute.Name,
                        Order = productAttribute.Order,
                        Items = productAttribute.ProductAttributeItems.OrEmptyIfNull().Select(x => new ProductAttributeItemResponseModel
                        {
                            Id = x.Id,
                            ProductAttributeId = x.ProductAttributeId,
                            Name = x.Name,
                            Order = x.Order,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        }),
                        LastModifiedDate = productAttribute.LastModifiedDate,
                        CreatedDate = productAttribute.CreatedDate
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
        /// Delete product attribute by id.
        /// </summary>
        /// <param name="id">The product attribute id.</param>
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

            var serviceModel = new DeleteProductAttributeServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteProductAttributeModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.productAttributesService.DeleteProductAttributeAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
