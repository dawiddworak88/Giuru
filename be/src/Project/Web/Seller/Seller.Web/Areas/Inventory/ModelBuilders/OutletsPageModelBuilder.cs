using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Areas.Inventory.ViewModel;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Inventory.ModelBuilders
{
    public class OutletsPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OutletsPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<OutletItem>> outletCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public OutletsPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<OutletItem>> outletCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.outletCatalogModelBuilder = outletCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<OutletsPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OutletsPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                Catalog = await this.outletCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
