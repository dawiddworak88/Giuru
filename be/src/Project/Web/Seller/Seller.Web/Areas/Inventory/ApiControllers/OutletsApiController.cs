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
using Seller.Web.Areas.Inventory.Repositories;
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
    public class OutletsApiController : BaseApiController
    {
        private readonly IOutletRepository outletRepository;
        private readonly IStringLocalizer inventoryLocalizer;
        private readonly IProductsRepository productsRepository;

        public OutletsApiController(
            IOutletRepository outletRepository,
            IStringLocalizer<InventoryResources> inventoryLocalizer,
            IProductsRepository productsRespository)
        {
            this.outletRepository = outletRepository;
            this.inventoryLocalizer = inventoryLocalizer;
            this.productsRepository = productsRespository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var outletItems = await this.outletRepository.GetAsync(
                token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(OutletItem.ProductSku)} ASC");

            return this.StatusCode((int)HttpStatusCode.OK, outletItems);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.outletRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.inventoryLocalizer.GetString("OutletItemDeletedSuccessfully").Value });
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveOutletRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language    = CultureInfo.CurrentUICulture.Name;

            var OrganisationId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value);
            var product = await this.productsRepository.GetProductAsync(token, language, model.ProductId);

            if (product != null)
            {
                var outletitemId = await this.outletRepository.SaveAsync(
                    token, language, model.Id, model.WarehouseId, model.ProductId, product.Name, 
                    product.Sku, model.Quantity, model.Title, model.Description,  model.Ean, model.AvailableQuantity, OrganisationId);

                return this.StatusCode((int)HttpStatusCode.OK, new { Id = outletitemId, Message = this.inventoryLocalizer.GetString("OutletItemSavedSuccessfully").Value });
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest, new { Message = $"{this.inventoryLocalizer.GetString("CannotFindProductById").Value} {model.ProductId}" });
        }
    }
}
