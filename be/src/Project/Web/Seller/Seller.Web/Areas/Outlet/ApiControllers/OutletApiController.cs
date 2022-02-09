using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Outlet.DomainModels;
using Seller.Web.Areas.Outlet.Repositories;
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

        public OutletApiController(
            IOutletRepository outletRepository,
            IStringLocalizer<InventoryResources> inventoryLocalizer)
        {
            this.outletRepository = outletRepository;
            this.inventoryLocalizer = inventoryLocalizer;
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
    }
}
