using Api.v1.Areas.Products.RequestModels;
using Api.v1.Areas.Products.ResponseModels;
using Feature.Account.Definitions;
using Feature.Product.Models;
using Feature.Product.Services;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Helpers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.v1.Areas.Products.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class ProductController : BaseApiController
    {
        private readonly IProductService productService;

        private readonly ILogger logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            this.productService = productService;
            this.logger = logger;
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
            try
            {
                var tenantClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.TenantIdClaim);

                var createUpdateModel = new CreateUpdateProductModel
                {
                    Id = request.Id,
                    Sku = request.Sku,
                    Name = request.Name,
                    FormData = request.FormData,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    TenantId = GuidHelper.ParseNullable(tenantClaim?.Value),
                    Language = request.Language
                };

                var resultModel = request.Id.HasValue ? await this.productService.UpdateAsync(createUpdateModel) : await this.productService.CreateAsync(createUpdateModel);
                
                if (resultModel.IsValid)
                {
                    return this.StatusCode((int)HttpStatusCode.Created, new ProductResponseModel(resultModel.Product));
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
                }
            }
            catch (Exception exception)
            {
                var error = ErrorHelper.GenerateErrorSignature(Assembly.GetExecutingAssembly().ToString());
                this.logger.LogError(exception, $"{error.ErrorId} - {error.ErrorSource}");
                return this.StatusCode((int)HttpStatusCode.BadRequest, new ProductResponseModel { Error = error });
            }
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
            try
            {
                var tenantClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.TenantIdClaim);

                var getProductModel = new GetProductModel
                {
                    Id = id,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    TenantId = GuidHelper.ParseNullable(tenantClaim?.Value),
                    Language = language
                };

                var getProductResult = await this.productService.GetByIdAsync(getProductModel);

                if (getProductResult.IsValid)
                {
                    return this.StatusCode((int)HttpStatusCode.OK, new ProductResponseModel(getProductResult.Product));
                }
                else
                {
                    if (getProductResult.Errors.Contains(ErrorConstants.NotFound))
                    {
                        return this.StatusCode((int)HttpStatusCode.NotFound);
                    }

                    return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
                }
            }
            catch (Exception exception)
            {
                var error = ErrorHelper.GenerateErrorSignature(Assembly.GetExecutingAssembly().ToString());
                this.logger.LogError(exception, $"{error.ErrorId} - {error.ErrorSource}");
                return this.StatusCode((int)HttpStatusCode.BadRequest, new ProductResponseModel { Error = error });
            }
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
            try
            {
                var tenantClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.TenantIdClaim);

                var deleteProductModel = new DeleteProductModel
                {   
                    Language = language,
                    Id = id,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    TenantId = GuidHelper.ParseNullable(tenantClaim?.Value)
                };

                var deleteProductResult = await this.productService.DeleteAsync(deleteProductModel);

                if (deleteProductResult.IsValid)
                {
                    return this.StatusCode((int)HttpStatusCode.OK);
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
                }
            }
            catch (Exception exception)
            {
                var error = ErrorHelper.GenerateErrorSignature(Assembly.GetExecutingAssembly().ToString());
                this.logger.LogError(exception, $"{error.ErrorId} - {error.ErrorSource}");
                return this.StatusCode((int)HttpStatusCode.BadRequest, new ProductResponseModel { Error = error });
            }
        }
    }
}
