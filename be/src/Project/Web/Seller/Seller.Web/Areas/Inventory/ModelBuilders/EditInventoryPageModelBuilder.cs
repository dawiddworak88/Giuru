using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using System.Globalization;
using Seller.Web.Areas.Inventory.ViewModel;

namespace Seller.Web.Areas.Inventory.ModelBuilders
{
    public class EditInventoryPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, EditInventoryPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, EditInventoryFormViewModel> inventoryFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public EditInventoryPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, EditInventoryFormViewModel> inventoryFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.inventoryFormModelBuilder = inventoryFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<EditInventoryPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new EditInventoryPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = this.headerModelBuilder.BuildModel(),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                InventoryForm = await this.inventoryFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
