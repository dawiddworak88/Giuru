using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using System.Globalization;
using Seller.Web.Areas.Inventory.ViewModel;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Inventory.ModelBuilders
{
    public class InventoryPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, InventoryPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, InventoryFormViewModel> inventoryFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public InventoryPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, InventoryFormViewModel> inventoryFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.inventoryFormModelBuilder = inventoryFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<InventoryPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new InventoryPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                InventoryForm = await this.inventoryFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
