using Buyer.Web.Areas.Products.Services.Products;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class ProductsApiController : BaseApiController
    {
        private readonly IProductsService productsService;

        public ProductsApiController(
            IProductsService productsService)
        {
            this.productsService = productsService;
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

    }
}
