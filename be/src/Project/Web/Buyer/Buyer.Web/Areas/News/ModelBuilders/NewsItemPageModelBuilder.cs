using Buyer.Web.Areas.News.ViewModel;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.ModelBuilders
{
    public class NewsItemPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsItemPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NewsItemBreadcrumbsViewModel> breadcrumbsModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NewsItemDetailsViewModel> newsDetailsModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public NewsItemPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NewsItemDetailsViewModel> newsDetailsModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NewsItemBreadcrumbsViewModel> breadcrumbsModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.mainNavigationModelBuilder = mainNavigationModelBuilder;
            this.newsDetailsModelBuilder = newsDetailsModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.breadcrumbsModelBuilder = breadcrumbsModelBuilder;
        }

        public async Task<NewsItemPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsItemPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                Breadcrumbs = await this.breadcrumbsModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await this.mainNavigationModelBuilder.BuildModelAsync(componentModel),
                NewsItemDetails = await this.newsDetailsModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
