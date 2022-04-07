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
    public class RegulationsPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, RegulationsPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IOptionsMonitor<AppSettings> options;

        public RegulationsPageModelBuilder(
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

        public async Task<RegulationsPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new RegulationsPageViewModel
            {
                Header = this.headerModelBuilder.BuildModel(),
                Content = new ContentPageViewModel
                { 
                    Title = this.globalLocalizer.GetString("Regulations"),
                    Content = HttpUtility.UrlDecode(this.options.CurrentValue.Regulations)
                },
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
