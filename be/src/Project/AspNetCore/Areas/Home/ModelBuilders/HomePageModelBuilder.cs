using AspNetCore.Areas.Home.ViewModel;
using Foundation.Extensions.ModelBuilders;
using AspNetCore.Shared.Headers.ViewModels;
using Microsoft.Extensions.Localization;
using Feature.Localization;

namespace AspNetCore.Areas.Home.ModelBuilders
{
    public class HomePageModelBuilder : IModelBuilder<HomePageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;

        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public HomePageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder, 
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.globalLocalizer = globalLocalizer;
        }

        public HomePageViewModel BuildModel()
        {
            var viewModel = new HomePageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                Welcome = this.globalLocalizer["Close"],
                LearnMore = "Learn more"
            };

            return viewModel;
        }
    }
}
