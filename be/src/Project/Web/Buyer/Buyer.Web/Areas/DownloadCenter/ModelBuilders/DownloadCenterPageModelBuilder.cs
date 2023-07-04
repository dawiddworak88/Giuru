using Buyer.Web.Areas.DownloadCenter.ViewModel;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.ModelBuilders
{
    public class DownloadCenterPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCatalogViewModel> downloadCenterCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder;

        public DownloadCenterPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCatalogViewModel> downloadCenterCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder)
        {
            this.seoModelBuilder = seoModelBuilder;
            this.headerModelBuilder = headerModelBuilder;
            this.mainNavigationModelBuilder = mainNavigationModelBuilder;
            this.downloadCenterCatalogModelBuilder = downloadCenterCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<DownloadCenterPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadCenterPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Metadata = await this.seoModelBuilder.BuildModelAsync(componentModel),
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await this.mainNavigationModelBuilder.BuildModelAsync(componentModel),
                Catalog = await this.downloadCenterCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = await footerModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}
