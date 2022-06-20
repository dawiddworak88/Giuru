using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    public class ClientAccountManagersController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientAccountManagersPageViewModel> managersPageModelBuilder;

        public ClientAccountManagersController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientAccountManagersPageViewModel> managersPageModelBuilder)
        {
            this.managersPageModelBuilder = managersPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.managersPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}