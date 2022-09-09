using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Foundation.PageContent.ComponentModels;
using System.Globalization;
using Microsoft.AspNetCore.Routing;

namespace Buyer.Web.Shared.ModelBuilders.Sidebar
{
    public class SidebarModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel>
    {
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<InventoryResources> inventoryLocalizer;
        private readonly LinkGenerator linkGenerator;

        public SidebarModelBuilder(
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<InventoryResources> inventoryLocalizer,
            LinkGenerator linkGenerator)
        {
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.inventoryLocalizer = inventoryLocalizer;
        }

        public async Task<SidebarViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new SidebarViewModel
            {
                SidebarTitle = this.orderLocalizer.GetString("SidebarTitle"),
                BasketUrl = this.linkGenerator.GetPathByAction("Index", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                ToBasketLabel = this.orderLocalizer.GetString("ToBasketLabel"),
                NoContent = this.orderLocalizer.GetString("NoContent"),
                FabricsLabel = this.orderLocalizer.GetString("FabricsLabel"),
                SkuLabel = this.globalLocalizer.GetString("Sku"),
                LackInformation = this.orderLocalizer.GetString("LackInformation"),
                ProductsApiUrl = this.linkGenerator.GetPathByAction("GetProductVariants", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                AddToCartLabel = this.orderLocalizer.GetString("AddToCart"),
                GoToDetailsLabel = this.orderLocalizer.GetString("GoToDetails"),
                InStockLabel = this.globalLocalizer.GetString("InStock"),
                ExpectedDeliveryLabel = this.inventoryLocalizer.GetString("ExpectedDeliveryLabel"),
                LoadingLabel = this.globalLocalizer.GetString("LoadingLabel"),
                InOutletLabel = this.globalLocalizer.GetString("InOutlet"),
                EanLabel = this.globalLocalizer.GetString("Ean")
            };
           
            return viewModel;
        }
    }
}
