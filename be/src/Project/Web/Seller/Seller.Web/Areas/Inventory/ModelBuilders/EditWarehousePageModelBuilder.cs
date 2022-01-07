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
    public class EditWarehousePageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, EditWarehousePageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, EditWarehouseFormViewModel> warehouseFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public EditWarehousePageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, EditWarehouseFormViewModel> warehouseFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.warehouseFormModelBuilder = warehouseFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<EditWarehousePageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new EditWarehousePageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = this.headerModelBuilder.BuildModel(),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                WarehouseForm = await this.warehouseFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
