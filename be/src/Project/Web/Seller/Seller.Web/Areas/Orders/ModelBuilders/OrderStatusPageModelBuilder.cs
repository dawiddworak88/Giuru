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
    public class OrderStatusPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderStatusPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrderStatusDetailViewModel> orderStatusDetailModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public OrderStatusPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, OrderStatusDetailViewModel> orderStatusDetailModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.orderStatusDetailModelBuilder = orderStatusDetailModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<OrderStatusPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderStatusPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                OrderStatusDetail = await this.orderStatusDetailModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
