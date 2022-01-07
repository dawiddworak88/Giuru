using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Identity.Api.Areas.Home.ViewModels;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Home.ModelBuilders
{
    public class RegulationsPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, RegulationsPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public RegulationsPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.globalLocalizer = globalLocalizer;
        }

        public async Task<RegulationsPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new RegulationsPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                Content = new ContentPageViewModel
                { 
                    Title = this.globalLocalizer.GetString("Regulations")
                },
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
