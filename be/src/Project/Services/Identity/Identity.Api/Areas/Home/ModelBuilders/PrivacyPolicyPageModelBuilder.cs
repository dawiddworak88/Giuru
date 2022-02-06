using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Identity.Api.Areas.Home.ViewModels;
using Identity.Api.Configurations;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Web;

namespace Identity.Api.Areas.Home.ModelBuilders
{
    public class PrivacyPolicyPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, PrivacyPolicyPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IOptionsMonitor<AppSettings> options;

        public PrivacyPolicyPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IOptionsMonitor<AppSettings> options)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.globalLocalizer = globalLocalizer;
            this.options = options;
        }

        public async Task<PrivacyPolicyPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new PrivacyPolicyPageViewModel
            {
                Header = this.headerModelBuilder.BuildModel(),
                Content = new ContentPageViewModel
                {
                    Title = this.globalLocalizer.GetString("PrivacyPolicy"),
                    Content = HttpUtility.UrlDecode(this.options.CurrentValue.PrivacyPolicy)
                },
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
