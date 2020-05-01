using Feature.Account.Definitions;
using Feature.Product.Models;
using Feature.Product.Services;
using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Product.Api.v1.RequestModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Api.v1.Controllers
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

        public ProductController(IProductService clientService, ILogger<ProductController> logger)
        {
            this.productService = clientService;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="productModel">Product to save.</param>
        /// <returns>Product creation results.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ProductRequestModel productModel)
        {
            try
            {
                var tenantClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.TenantIdClaim);

                var createProductModel = new CreateProductModel
                {
                    TenantId = !string.IsNullOrWhiteSpace(tenantClaim?.Value) ? Guid.Parse(tenantClaim.Value) : Guid.Empty,
                    Sku = productModel.Sku,
                    Language = productModel.Language
                };

                var createProductResult = await this.productService.CreateAsync(createProductModel);

                if (createProductResult.ValidationResult.IsValid)
                {
                    return this.Ok();
                }

                return this.BadRequest(createProductResult.ValidationResult.Errors.ToString());
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, nameof(Create));
            }

            return this.BadRequest();
        }
    }
}
