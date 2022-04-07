using Seller.Web.Areas.Orders.ViewModel;
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
    public class InventoriesController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, InventoriesPageViewModel> inventoriesPageModelBuilder;

        public InventoriesController(IAsyncComponentModelBuilder<ComponentModelBase, InventoriesPageViewModel> inventoriesPageModelBuilder)
        {
            this.inventoriesPageModelBuilder = inventoriesPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.inventoriesPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
