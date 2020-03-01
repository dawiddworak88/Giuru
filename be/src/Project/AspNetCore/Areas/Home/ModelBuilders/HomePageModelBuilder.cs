using AspNetCore.Areas.Home.ViewModel;
using AspNetCore.Extensions.ModelBuilders;
using AspNetCore.Shared.Headers.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;

namespace AspNetCore.Areas.Home.ModelBuilders
{
    public class HomeModelBuilder : IModelBuilder<HomePageViewModel>
    {
        private readonly IStringLocalizer<HomeModelBuilder> globalLocalizer;
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly LinkGenerator linkGenerator;

        public HomeModelBuilder(
            IStringLocalizer<HomeModelBuilder> globalLocalizer,
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.headerModelBuilder = headerModelBuilder;
            this.linkGenerator = linkGenerator;
        }

        public HomePageViewModel BuildModel()
        {
            var viewModel = new HomePageViewModel
            {
                Header = headerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
