using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Settings.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    public class SettingsController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SettingsPageViewModel> settingsPageModelBuilder;

        public SettingsController(IAsyncComponentModelBuilder<ComponentModelBase, SettingsPageViewModel> settingsPageModelBuilder)
        {
            this.settingsPageModelBuilder = settingsPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.settingsPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
