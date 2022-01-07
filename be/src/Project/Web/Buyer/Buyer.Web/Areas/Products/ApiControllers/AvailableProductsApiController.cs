using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class AvailableProductsApiController : BaseApiController
    {
        private readonly IProductsService productsService;
        private readonly IInventoryRepository inventoryRepository;

        public AvailableProductsApiController(
            IProductsService productsService,
            IInventoryRepository inventoryRepository)
        {
            this.productsService = productsService;
            this.inventoryRepository = inventoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? categoryId, Guid? brandId, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var inventories = await this.inventoryRepository.GetAvailbleProductsInventory(
                CultureInfo.CurrentUICulture.Name,
                pageIndex,
                itemsPerPage,
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName));

            if (inventories?.Data is not null && inventories.Data.Any())
            {
                var products = await this.productsService.GetProductsAsync(
                    inventories.Data.Select(x => x.ProductId),
                    null,
                    null,
                    CultureInfo.CurrentUICulture.Name,
                    null,
                    pageIndex,
                    itemsPerPage,
                    await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName));

                if (products is not null)
                {
                    foreach (var product in products.Data)
                    {
                        product.InStock = true;
                        product.AvailableQuantity = inventories.Data.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;
                    }

                    return this.StatusCode((int)HttpStatusCode.OK, new PagedResults<IEnumerable<CatalogItemViewModel>>(inventories.Total, itemsPerPage) { Data = products.Data.OrderByDescending(x => x.AvailableQuantity) });
                }
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
