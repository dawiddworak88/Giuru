using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Outlet.ApiRequestModels;
using Seller.Web.Areas.Outlet.DomainModels;
using Seller.Web.Areas.Outlet.Repositories;
using Seller.Web.Areas.Shared.Repositories.Products;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Outlet.ApiControllers
{
    [Area("Outlet")]
    public class OutletApiController : BaseApiController
    {
        private readonly IOutletRepository outletRepository;
        private readonly IStringLocalizer inventoryLocalizer;
        private readonly IProductsRepository productsRespository;

        public OutletApiController(
            IOutletRepository outletRepository,
            IStringLocalizer<InventoryResources> inventoryLocalizer,
            IProductsRepository productsRespository)
        {
            this.outletRepository = outletRepository;
            this.inventoryLocalizer = inventoryLocalizer;
            this.productsRespository = productsRespository;
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

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.inventoryLocalizer.GetString("InventoryDeletedSuccessfully").Value });
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] OutletRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language    = CultureInfo.CurrentUICulture.Name;

            var product = await this.productsRespository.GetProductAsync(token, language, model.ProductId);
            if (product != null)
            {
                var outletitemId = await this.outletRepository.SaveAsync(
                    token, language, model.Id, model.ProductId, model.ProductName, model.ProductSku);

                return this.StatusCode((int)HttpStatusCode.OK, new { Id = outletitemId, Message = this.inventoryLocalizer.GetString("InventorySavedSuccessfully").Value });
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest, new { Message = $"{this.inventoryLocalizer.GetString("CannotFindProductById").Value} {model.ProductId}" });
        }
    }
}
