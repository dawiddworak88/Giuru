using Catalog.Api.v1.Areas.Products.RequestModels;
using Catalog.Api.v1.Areas.Products.Models;
using Catalog.Api.v1.Areas.Products.Services;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Foundation.Account.Definitions;
using Catalog.Api.v1.Areas.Products.Validators;

namespace Catalog.Api.v1.Areas.Products.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class ProductController : BaseApiController
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        /// <summary>
        /// Saves the product. Performs create if id is null and update otherwise.
        /// </summary>
        /// <param name="request">Product to save.</param>
        /// <returns>Product creation result.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Save([FromBody] ProductRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var serviceModel = new CreateUpdateProductModel
            {
                Id = request.Id,
                PrimaryProductId = request.PrimaryProductId,
                Sku = request.Sku,
                Name = request.Name,
                Description = request.Description,
                FormData = request.FormData,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Language = request.Language
            };

            if (request.Id.HasValue)
            {
                var validator = new UpdateProductModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var product = await this.productService.UpdateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, product);
                }
            }
            else
            {
                var validator = new CreateProductModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var product = await this.productService.CreateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.Created, product);
                }
            }

            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Returns a product by id.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="id">The product id.</param>
        /// <returns>The product.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(string language, Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var serviceModel = new GetProductModel
            {
                Id = id,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Language = language
            };

            var validator = new GetProductModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var product = await this.productService.GetByIdAsync(serviceModel);

                return product != null ? this.StatusCode((int)HttpStatusCode.OK, product) : (IActionResult)this.StatusCode((int)HttpStatusCode.NotFound);
            }

            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Deletes the product by id.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="id">The id of a product to delete.</param>
        /// <returns>The deletion process result.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Delete(string language, Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var serviceModel = new DeleteProductModel
            {   
                Language = language,
                Id = id,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteProductModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.productService.DeleteAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
