using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Identity.Api.Areas.Home.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Home.Controllers
{
    [Area("Home")]
    [AllowAnonymous]
    public class ContentController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, PrivacyPolicyPageViewModel> privacyPolicyPageModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, RegulationsPageViewModel> regulationsPageModelBuilder;

        public ContentController(
            IAsyncComponentModelBuilder<ComponentModelBase, PrivacyPolicyPageViewModel> privacyPolicyPageModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, RegulationsPageViewModel> regulationsPageModelBuilder)
        {
            this.privacyPolicyPageModelBuilder = privacyPolicyPageModelBuilder;
            this.regulationsPageModelBuilder = regulationsPageModelBuilder;
        }

        public async Task<IActionResult> PrivacyPolicy()
        {
            var componentModel = new ComponentModelBase
            {
                ContentPageKey = "privacyPolicyPage",
                Language = CultureInfo.CurrentUICulture.Name
            };

            var viewModel = await this.privacyPolicyPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Regulations()
        {
            var componentModel = new ComponentModelBase
            {
                ContentPageKey = "regulationsPage",
                Language = CultureInfo.CurrentUICulture.Name
            };

            var viewModel = await this.regulationsPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
