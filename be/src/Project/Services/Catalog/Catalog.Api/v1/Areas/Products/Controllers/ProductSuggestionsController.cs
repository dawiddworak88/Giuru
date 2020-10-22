using Catalog.Api.v1.Areas.Products.Models;
using Catalog.Api.v1.Areas.Products.Services;
using Catalog.Api.v1.Areas.Products.Validators;
using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Products.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [AllowAnonymous]
    [ApiController]
    public class ProductSuggestionsController : BaseApiController
    {
        private readonly IProductService productService;

        public ProductSuggestionsController(IProductService productService)
        {
            this.productService = productService;
        }

        /// <summary>
        /// Gets product suggestions by search term.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="size">The maximum number of items.</param>
        /// <param name="language">The language.</param>
        /// <returns>The list of suggestions</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get(string searchTerm, int size, string language)
        {
            var serviceModel = new GetProductSuggestionsModel
            {
                SearchTerm = searchTerm,
                Size = size,
                Language = language
            };

            var validator = new GetProductSuggestionsModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var suggestions = await this.productService.GetProductSuggestionsAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK, suggestions);
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
