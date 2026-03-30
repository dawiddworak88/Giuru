using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.Services.DeliveryMessages;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.Sidebar
{
    public class SidebarModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel>
    {
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<InventoryResources> inventoryLocalizer;
        private readonly IDeliveryMessageHelper _deliveryMessageHelper;
        private readonly LinkGenerator linkGenerator;

        public SidebarModelBuilder(
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<InventoryResources> inventoryLocalizer,
            IDeliveryMessageHelper deliveryMessageHelper,
            LinkGenerator linkGenerator)
        {
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.inventoryLocalizer = inventoryLocalizer;
            _deliveryMessageHelper = deliveryMessageHelper;
        }

        public async Task<SidebarViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new SidebarViewModel
            {
                SidebarTitle = this.orderLocalizer.GetString("SidebarTitle"),
                BasketUrl = this.linkGenerator.GetPathByAction("Index", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                ToBasketLabel = this.orderLocalizer.GetString("ToBasketLabel"),
                NotFound = this.orderLocalizer.GetString("NotFound"),
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

            if (componentModel is PriceComponentModel priceComponentModel)
            {
                viewModel.LeadTimeDeliveryMessage = _deliveryMessageHelper.GetDeliveryMessage(priceComponentModel.DeliveryType, false);
            }

            return viewModel;
        }
    }
}
