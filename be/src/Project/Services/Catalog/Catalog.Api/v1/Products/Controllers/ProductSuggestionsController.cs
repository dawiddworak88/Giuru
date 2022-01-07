using Catalog.Api.ServicesModels.Products;
using Catalog.Api.Services.Products;
using Catalog.Api.Validators.Products;
using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Catalog.Api.v1.Products.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [AllowAnonymous]
    [ApiController]
    public class ProductSuggestionsController : BaseApiController
    {
        private readonly IProductsService productService;

        public ProductSuggestionsController(IProductsService productService)
        {
            this.productService = productService;
        }

        /// <summary>
        /// Gets product suggestions by search term.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="size">The maximum number of items.</param>
        /// <returns>The list of suggestions</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<string>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(string searchTerm, int size)
        {
            var serviceModel = new GetProductSuggestionsServiceModel
            {
                SearchTerm = searchTerm,
                Size = size,
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new GetProductSuggestionsModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var suggestions = this.productService.GetProductSuggestions(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK, suggestions);
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
