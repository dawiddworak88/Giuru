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
        /// Returns products by search term. Returns all products (paginated) if search term is empty.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <returns></returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get(string language, string searchTerm, int pageIndex, int itemsPerPage)
        {
            try
            {
                var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.SellerIdClaim);

                var getProductsModel = new GetProductsModel
                {
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    SearchTerm = searchTerm,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    SellerId = GuidHelper.ParseNullable(sellerClaim?.Value),
                    Language = language
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
