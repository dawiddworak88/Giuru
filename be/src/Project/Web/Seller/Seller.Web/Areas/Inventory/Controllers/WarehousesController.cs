using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using System.Globalization;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Foundation.Extensions.Helpers;
using System.Linq;
using Foundation.Account.Definitions;
using Seller.Web.Areas.Inventory.ViewModel;

namespace Seller.Web.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class WarehousesController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, WarehousesPageViewModel> warehousesPageModelBuilder;

        public WarehousesController(IAsyncComponentModelBuilder<ComponentModelBase, WarehousesPageViewModel> warehousesPageModelBuilder)
        {
            this.warehousesPageModelBuilder = warehousesPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.warehousesPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
