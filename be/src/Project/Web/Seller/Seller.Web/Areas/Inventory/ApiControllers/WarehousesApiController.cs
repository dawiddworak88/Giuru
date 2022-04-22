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
using Seller.Web.Areas.Inventory.Repositories.Warehouses;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Inventory.ApiControllers
{
    [Area("Inventory")]
    public class WarehousesApiController : BaseApiController
    {
        private readonly IWarehousesRepository warehousesRepository;
        private readonly IStringLocalizer inventoryLocalizer;

        public WarehousesApiController(
            IWarehousesRepository warehousesRepository,
            IStringLocalizer<InventoryResources> inventoryLocalizer)
        {
            this.inventoryLocalizer = inventoryLocalizer;
            this.warehousesRepository = warehousesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var warehouses = await this.warehousesRepository.GetWarehousesAsync(token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(Warehouse.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, warehouses);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveWarehouseRequestModel model)
        {
            var OrganisationId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value);
            var warehouseId = await this.warehousesRepository.SaveAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name, model.Id, model.Name, model.Location, OrganisationId);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = warehouseId, Message = this.inventoryLocalizer.GetString("WarehouseSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.warehousesRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.inventoryLocalizer.GetString("WarehouseDeletedSuccessfully").Value });
        }
    }
}
