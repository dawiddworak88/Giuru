using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Helpers;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Inventory.ApiRequestModels;
using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Areas.Inventory.Repositories.Inventories;
using Seller.Web.Areas.Shared.Repositories.Products;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Inventory.ApiControllers
{
    [Area("Inventory")]
    public class InventoriesApiController : BaseApiController
    {
        private readonly IProductsRepository productsRepository;
        private readonly IInventoryRepository inventoriesRepository;
        private readonly IStringLocalizer inventoryLocalizer;

        public InventoriesApiController(
            IProductsRepository productsRepository,
            IInventoryRepository inventoriesRepository,
            IStringLocalizer<InventoryResources> inventoryLocalizer)
        {
            this.inventoriesRepository = inventoriesRepository;
            this.inventoryLocalizer = inventoryLocalizer;
            this.productsRepository = productsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var inventories = await this.inventoriesRepository.GetInventoryProductsAsync(token,
                language,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(InventoryItem.ProductSku)} ASC");

            return this.StatusCode((int)HttpStatusCode.OK, inventories);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveInventoryRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var OrganisationId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim)?.Value);
            var product = await this.productsRepository.GetProductAsync(token, language, model.ProductId);

            if (product != null)
            {
                var inventoryId = await this.inventoriesRepository.SaveAsync(
                    token, language, model.Id, model.WarehouseId, model.ProductId,
                    product.Name, product.Sku, model.Quantity, model.RestockableInDays, 
                    model.AvailableQuantity, model.ExpectedDelivery, OrganisationId);

                return this.StatusCode((int)HttpStatusCode.OK, new { Id = inventoryId, Message = this.inventoryLocalizer.GetString("InventorySavedSuccessfully").Value });
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest, new { Message = $"{this.inventoryLocalizer.GetString("CannotFindProductById").Value} {model.ProductId}" } );
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.inventoriesRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.inventoryLocalizer.GetString("InventoryDeletedSuccessfully").Value });
        }
    }
}
