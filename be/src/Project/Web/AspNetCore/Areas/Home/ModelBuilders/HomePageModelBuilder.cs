using AspNetCore.Areas.Home.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Feature.PageContent.Shared.Headers.ViewModels;
using Feature.PageContent.Shared.Footers.ViewModels;

namespace AspNetCore.Areas.Home.ModelBuilders
{
    public class HomePageModelBuilder : IModelBuilder<HomePageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;

        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public HomePageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.globalLocalizer = globalLocalizer;
        }

        public HomePageViewModel BuildModel()
        {
            var viewModel = new HomePageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                Footer = footerModelBuilder.BuildModel(),
                Welcome = this.globalLocalizer["Close"],
                LearnMore = "Learn more"
            };

            return viewModel;
        }
    }
}
