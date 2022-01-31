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
        private readonly IStringLocalizer<OrderResources> orderResources;
        private readonly IStringLocalizer<GlobalResources> globalResources;
        private readonly LinkGenerator linkGenerator;

        public SidebarModelBuilder(
            IStringLocalizer<OrderResources> orderResources,
            IStringLocalizer<GlobalResources> globalResources,
            LinkGenerator linkGenerator)
        {
            this.orderResources = orderResources;
            this.linkGenerator = linkGenerator;
            this.globalResources = globalResources;
        }

        public async Task<SidebarViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new SidebarViewModel
            {
                SidebarTitle = this.orderResources.GetString("SidebarTitle"),
                BasketUrl = this.linkGenerator.GetPathByAction("Index", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                ToBasketLabel = this.orderResources.GetString("ToBasketLabel"),
                NotFound = this.orderResources.GetString("NotFound"),
                FabricsLabel = this.orderResources.GetString("FabricsLabel"),
                SkuLabel = this.globalResources.GetString("Sku"),
                LackInformation = this.orderResources.GetString("LackInformation"),
                ProductsApiUrl = this.linkGenerator.GetPathByAction("GetProductVariants", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
            };
           
            return viewModel;
        }
    }
}
