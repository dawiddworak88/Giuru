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
    public class InventoriesPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, InventoriesPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<InventoryItem>> inventoriesCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public InventoriesPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<InventoryItem>> inventoriesCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.inventoriesCatalogModelBuilder = inventoriesCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<InventoriesPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new InventoriesPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = this.headerModelBuilder.BuildModel(),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                Catalog = await this.inventoriesCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
