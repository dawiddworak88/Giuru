using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Inventory.ViewModel;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class InventoriesController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, InventoriesPageViewModel> inventoriesPageModelBuilder;

        public InventoriesController(
            IAsyncComponentModelBuilder<ComponentModelBase, InventoriesPageViewModel> inventoriesPageModelBuilder)
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
