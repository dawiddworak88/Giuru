using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
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
    public class SearchProductsApiController : BaseApiController
    {
        private readonly IProductsService _productsService;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IOutletRepository _outletRepository;

        public SearchProductsApiController(
            IProductsService productsService,
            IOutletRepository outletRepository,
            IInventoryRepository inventoryRepository)
        {
            _productsService = productsService;
            _inventoryRepository = inventoryRepository;
            _outletRepository = outletRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var products = await _productsService.GetProductsAsync(null, null, null, language, searchTerm, true, pageIndex, itemsPerPage, token, orderBy);
            
            if (products.Data is not null)
            {
                var outletItems = await _outletRepository.GetOutletProductsByIdsAsync(token, language, products.Data.Select(x => x.Id));
                var inventoryItems = await _inventoryRepository.GetAvailbleProductsInventoryByIds(token, language, products.Data.Select(x => x.Id));

                foreach (var product in products.Data.OrEmptyIfNull())
                {
                    var outletItem = outletItems.FirstOrDefault(x => x.ProductSku == product.Sku);

                    if (outletItem is not null)
                    {
                        product.InOutlet = true;
                        product.AvailableOutletQuantity = outletItem.AvailableQuantity;
                        product.OutletTitle = outletItem.Title;
                    }

                    var inventoryItem = inventoryItems.FirstOrDefault(x => x.ProductSku == product.Sku);

                    if (inventoryItem is not null)
                    {
                        product.InStock = true;
                        product.AvailableQuantity = inventoryItem.AvailableQuantity;
                        product.ExpectedDelivery = inventoryItem.ExpectedDelivery;
                    }

                    product.CanOrder = true;
                }

                var response = new PagedResults<IEnumerable<CatalogItemViewModel>>(products.Total, products.PageSize)
                {
                    Data = products.Data
                };

                return StatusCode((int)HttpStatusCode.OK, response);
            }

            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
