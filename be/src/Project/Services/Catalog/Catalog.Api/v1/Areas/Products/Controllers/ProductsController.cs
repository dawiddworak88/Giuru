using Catalog.Api.v1.Areas.Products.ResponseModels;
using Catalog.Api.v1.Areas.Products.Models;
using Catalog.Api.v1.Areas.Products.Services;
using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Catalog.Api.v1.Areas.Products.Validators;

namespace Catalog.Api.v1.Areas.Products.Controllers
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
        /// <param name="categoryId">The category id.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <returns></returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string language, Guid? categoryId, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var serviceModel = new GetProductsModel
            {
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                SearchTerm = searchTerm,
                CategoryId = categoryId,
                Language = language
            };

            var validator = new GetProductsModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var products = await this.productService.GetAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK, new ProductsResponseModel(products));
            }

            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
