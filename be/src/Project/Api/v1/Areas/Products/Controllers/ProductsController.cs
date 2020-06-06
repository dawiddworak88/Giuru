using Api.v1.Areas.Products.RequestModels;
using Api.v1.Areas.Products.ResponseModels;
using Feature.Account.Definitions;
using Feature.Product.Models;
using Feature.Product.Services;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Helpers;
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
    public class ProductsController : BaseApiController
    {
        private readonly IProductService productService;

        private readonly ILogger logger;

        public ProductsController(IProductService productService, ILogger<ProductController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        /// <summary>
        /// Returns a product by id.
        /// </summary>
        /// <param name="productModel">Products to get.</param>
        /// <returns>The product.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get([FromBody] GetProductsRequestModel productModel)
        {
            try
            {
                var tenantClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.TenantIdClaim);

                var getProductsModel = new GetProductsModel
                {
                    PageIndex = productModel.PageIndex,
                    ItemsPerPage = productModel.ItemsPerPage,
                    SearchTerm = productModel.SearchTerm,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    TenantId = GuidHelper.ParseNullable(tenantClaim?.Value),
                    Language = productModel.Language
                };

                var getProductsResult = await this.productService.GetAsync(getProductsModel);

                if (getProductsResult.IsValid)
                {
                    return this.StatusCode((int)HttpStatusCode.OK, new ProductsResponseModel(getProductsResult.Products));
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
