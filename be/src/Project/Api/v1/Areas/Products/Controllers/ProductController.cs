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
        /// Creates a new product
        /// </summary>
        /// <param name="productModel">Product to save.</param>
        /// <returns>Client creation results.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Create([FromBody] ProductRequestModel productModel)
        {
            try
            {
                var tenantClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.TenantIdClaim);

                var createProductModel = new CreateProductModel
                {
                    Sku = productModel.Sku,
                    Name = productModel.Name,
                    SchemaId = productModel.SchemaId,
                    FormData = productModel.FormData,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    TenantId = GuidHelper.ParseNullable(tenantClaim?.Value),
                    Language = productModel.Language
                };

                var createProductResult = await this.productService.CreateAsync(createProductModel);

                if (createProductResult.IsValid)
                {
                    return this.StatusCode((int)HttpStatusCode.Created, new ProductResponseModel(createProductResult.Product));
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
        /// <param name="productModel">Product to get.</param>
        /// <returns>The product.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById([FromBody] GetProductRequestModel productModel)
        {
            try
            {
                var tenantClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.TenantIdClaim);

                var getProductModel = new GetProductModel
                {
                    Id = productModel.Id,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    TenantId = GuidHelper.ParseNullable(tenantClaim?.Value),
                    Language = productModel.Language
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
    }
}
