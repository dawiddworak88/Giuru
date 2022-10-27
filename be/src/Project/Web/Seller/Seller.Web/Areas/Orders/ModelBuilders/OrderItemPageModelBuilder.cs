using Seller.Web.Areas.Orders.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using System.Globalization;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderItemPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderItemPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrderItemFormViewModel> orderItemFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public OrderItemPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, OrderItemFormViewModel> orderItemFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.orderItemFormModelBuilder = orderItemFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<OrderItemPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderItemPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                OrderItemForm = await this.orderItemFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
