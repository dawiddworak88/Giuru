using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.Definitions.Filters;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class OutletApiController : BaseApiController
    {
        private readonly IProductsService productsService;
        private readonly IOutletRepository outletRepository;

        public OutletApiController(
            IProductsService productsService,
            IOutletRepository outletRepository)
        {
            this.productsService = productsService;
            this.outletRepository= outletRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageIndex, int itemsPerPage)
        {
            var language = CultureInfo.CurrentUICulture.Name;
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);

            var outletItems = await this.outletRepository.GetOutletProductsAsync(language, pageIndex, itemsPerPage, token);

            if (outletItems?.Data is not null && outletItems.Data.Any())
            {
                var products = await this.productsService.GetProductsAsync(
                    outletItems.Data.Select(x => x.ProductId), null, null, language, null, false, pageIndex, itemsPerPage, token, null, SortingConstants.Default);

                if (products is not null)
                {
                    foreach (var product in products.Data)
                    {
                        var availableQuantity = outletItems.Data.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;

                        if (availableQuantity > 0)
                        {
                            product.CanOrder = true;
                            product.AvailableQuantity = availableQuantity;
                        }

                        product.InOutlet = true;
                        product.ExpectedDelivery = outletItems.Data.FirstOrDefault(x => x.ProductId == product.Id)?.ExpectedDelivery;
                    }

                    return this.StatusCode((int)HttpStatusCode.OK, new PagedResults<IEnumerable<CatalogItemViewModel>>(outletItems.Total, itemsPerPage) { Data = products.Data.OrderByDescending(x => x.AvailableQuantity) });
                }
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
