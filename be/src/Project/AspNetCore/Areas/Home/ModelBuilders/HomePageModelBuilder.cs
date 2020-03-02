using AspNetCore.Areas.Home.ViewModel;
using AspNetCore.Extensions.ModelBuilders;
using AspNetCore.Shared.Headers.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;

namespace AspNetCore.Areas.Home.ModelBuilders
{
    public class HomePageModelBuilder : IModelBuilder<HomePageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly LinkGenerator linkGenerator;

        public HomePageModelBuilder(IModelBuilder<HeaderViewModel> headerModelBuilder, LinkGenerator linkGenerator)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.linkGenerator = linkGenerator;
        }

        public HomePageViewModel BuildModel()
        {
            var viewModel = new HomePageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                Welcome = "Welcome",
                LearnMore = "Learn more"
            };

            return viewModel;
        }
    }
}
