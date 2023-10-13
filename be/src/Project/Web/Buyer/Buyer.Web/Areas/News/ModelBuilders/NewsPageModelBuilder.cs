using Buyer.Web.Areas.News.ViewModel;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.ModelBuilders
{
    public class NewsPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NewsCatalogViewModel> newsCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder;

        public NewsPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NewsCatalogViewModel> newsCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.mainNavigationModelBuilder = mainNavigationModelBuilder;
            this.newsCatalogModelBuilder = newsCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.seoModelBuilder = seoModelBuilder;
        }

        public async Task<NewsPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Metadata = await this.seoModelBuilder.BuildModelAsync(componentModel),
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await this.mainNavigationModelBuilder.BuildModelAsync(componentModel),
                NewsCatalog = await this.newsCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = await footerModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}
