using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Inventory.Repositories.Warehouses;
using Seller.Web.Areas.Inventory.ViewModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Inventory.ModelBuilders
{
    public class WarehouseFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, WarehouseFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<InventoryResources> warehouseLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IWarehousesRepository warehousesRepository;

        public WarehouseFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<InventoryResources> warehouseLocalizer,
            LinkGenerator linkGenerator,
            IWarehousesRepository warehousesRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.warehouseLocalizer = warehouseLocalizer;
            this.linkGenerator = linkGenerator;
            this.warehousesRepository = warehousesRepository;
        }

        public async Task<WarehouseFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new WarehouseFormViewModel
            {
                Title = this.warehouseLocalizer.GetString("EditWarehouses"),
                NameLabel = this.warehouseLocalizer.GetString("NameLabel"),
                LocationLabel = this.warehouseLocalizer.GetString("LocationLabel"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                NameRequiredErrorMessage = this.warehouseLocalizer.GetString("NameRequiredErrorMessage"),
                LocationRequiredErrorMessage = this.warehouseLocalizer.GetString("LocationRequiredErrorMessage"),
                NavigateToWarehouseListText = this.warehouseLocalizer.GetString("NavigateToWarehouseListText"),
                WarehouseUrl = this.linkGenerator.GetPathByAction("Index", "Warehouses", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name }),
                SaveText = this.warehouseLocalizer.GetString("SaveText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "WarehousesApi", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name }),
            };

            if (componentModel.Id.HasValue)
            {
                var warehouse = await this.warehousesRepository.GetWarehouseAsync(
                    componentModel.Token,
                    componentModel.Language,
                    componentModel.Id);

                if (warehouse != null)
                {
                    viewModel.Id = warehouse.Id;
                    viewModel.Name = warehouse.Name;
                    viewModel.Location = warehouse.Location;
                }
            }

            return viewModel;
        }
    }
}
