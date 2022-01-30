using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Inventory.ViewModel;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class InventoryController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, EditInventoryPageViewModel> editInventoryPageModelBuilder;

        public InventoryController(IAsyncComponentModelBuilder<ComponentModelBase, EditInventoryPageViewModel> editInventoryPageModelBuilder)
        {
            this.editInventoryPageModelBuilder = editInventoryPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.editInventoryPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
