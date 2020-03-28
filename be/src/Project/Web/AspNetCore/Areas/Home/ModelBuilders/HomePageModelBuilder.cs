using AspNetCore.Areas.Home.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.Components.Footers.ViewModels;

namespace AspNetCore.Areas.Home.ModelBuilders
{
    public class HomePageModelBuilder : IModelBuilder<HomePageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;

        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public HomePageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public HomePageViewModel BuildModel()
        {
            var viewModel = new HomePageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
