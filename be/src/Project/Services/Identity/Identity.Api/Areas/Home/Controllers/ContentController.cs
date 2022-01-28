using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Identity.Api.Areas.Home.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Home.Controllers
{
    [AllowAnonymous]
    public class ContentController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, PrivacyPolicyPageViewModel> privacyPolicyPageModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, RegulationsPageViewModel> regulationsPageModelBuilder;
        private readonly ILogger<ContentController> logger;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer; 

        public ContentController(
            IAsyncComponentModelBuilder<ComponentModelBase, PrivacyPolicyPageViewModel> privacyPolicyPageModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, RegulationsPageViewModel> regulationsPageModelBuilder,
            ILogger<ContentController> logger,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.privacyPolicyPageModelBuilder = privacyPolicyPageModelBuilder;
            this.regulationsPageModelBuilder = regulationsPageModelBuilder;
            this.logger = logger;
            this.globalLocalizer = globalLocalizer;
        }

        public async Task<IActionResult> PrivacyPolicy()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name
            };

            var viewModel = await this.privacyPolicyPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Regulations()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name
            };

            var viewModel = await this.regulationsPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
