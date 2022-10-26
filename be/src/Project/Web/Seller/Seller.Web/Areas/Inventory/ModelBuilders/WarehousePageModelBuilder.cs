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
    public class WarehousePageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, WarehousePageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, WarehouseFormViewModel> warehouseFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public WarehousePageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, WarehouseFormViewModel> warehouseFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.warehouseFormModelBuilder = warehouseFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<WarehousePageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new WarehousePageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                WarehouseForm = await this.warehouseFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
