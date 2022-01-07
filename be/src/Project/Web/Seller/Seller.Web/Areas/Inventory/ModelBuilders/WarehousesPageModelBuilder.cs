using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Shared.ViewModels;
using System.Threading.Tasks;
using System.Globalization;
using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Areas.Inventory.ViewModel;

namespace Seller.Web.Areas.Inventory.ModelBuilders
{
    public class WarehousesPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, WarehousesPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Warehouse>> warehousesCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public WarehousesPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Warehouse>> warehousesCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.warehousesCatalogModelBuilder = warehousesCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<WarehousesPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new WarehousesPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = this.headerModelBuilder.BuildModel(),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                Catalog = await this.warehousesCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
