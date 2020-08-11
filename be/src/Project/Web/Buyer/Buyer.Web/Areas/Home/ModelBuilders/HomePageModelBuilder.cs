using Buyer.Web.Areas.Home.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Buyer.Web.Shared.Headers.ViewModels;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HomePageModelBuilder : IModelBuilder<HomePageViewModel>
    {
        private readonly IModelBuilder<BuyerHeaderViewModel> headerModelBuilder;

        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public HomePageModelBuilder(
            IModelBuilder<BuyerHeaderViewModel> headerModelBuilder,
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
