using AspNetCore.Areas.Home.ViewModel;
using AspNetCore.Extensions.ModelBuilders;
using AspNetCore.Localization;
using AspNetCore.Shared.Headers.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;

namespace AspNetCore.Areas.Home.ModelBuilders
{
    public class HomePageModelBuilder : IModelBuilder<HomePageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;

        private readonly IStringLocalizer<Global> globalLocalizer;

        public HomePageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder, 
            IStringLocalizer<Global> globalLocalizer)
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
