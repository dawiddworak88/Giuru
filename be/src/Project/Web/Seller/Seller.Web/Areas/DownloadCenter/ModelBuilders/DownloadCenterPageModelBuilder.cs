using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.DownloadCenter.DomainModels;
using Seller.Web.Areas.DownloadCenter.ViewModel;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.ModelBuilders
{
    public class DownloadCenterPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<DownloadCenterItem>> downloadCenterCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public DownloadCenterPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<DownloadCenterItem>> downloadCenterCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.downloadCenterCatalogModelBuilder = downloadCenterCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<DownloadCenterPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadCenterPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                Catalog = await this.downloadCenterCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
