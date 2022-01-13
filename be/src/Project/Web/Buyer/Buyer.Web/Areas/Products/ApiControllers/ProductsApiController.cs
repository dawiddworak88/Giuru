using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.Repositories.Products.Suggestions;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class ProductsApiController : BaseApiController
    {
        private readonly IProductsService productsService;
        private readonly IProductSuggestionRepository productsRepository;

        public ProductsApiController(
            IProductsService productsService,
            IProductSuggestionRepository productsRepository)
        {
            this.productsService = productsService;
            this.productsRepository = productsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? categoryId, Guid? brandId, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var products = await this.productsService.GetProductsAsync(
                null,
                categoryId,
                brandId,
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                pageIndex,
                itemsPerPage,
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName));

            return this.StatusCode((int)HttpStatusCode.OK, products);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsSuggestion(string searchTerm, bool? hasPrimaryProduct, int pageIndex, int itemsPerPage, string orderBy)
        {
            var organisationId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim)?.Value);
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var products = await this.productsRepository.GetProductsAsync(
                token, language, searchTerm, hasPrimaryProduct, organisationId, pageIndex, itemsPerPage, orderBy);

            return this.StatusCode((int)HttpStatusCode.OK, products);
        }
    }
}
