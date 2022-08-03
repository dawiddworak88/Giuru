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
    public class DownloadCenterCategoriesPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoriesPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<DownloadCenterCategory>> categoriesCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public DownloadCenterCategoriesPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<DownloadCenterCategory>> categoriesCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.categoriesCatalogModelBuilder = categoriesCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<DownloadCenterCategoriesPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadCenterCategoriesPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = this.headerModelBuilder.BuildModel(),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                Catalog = await this.categoriesCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
