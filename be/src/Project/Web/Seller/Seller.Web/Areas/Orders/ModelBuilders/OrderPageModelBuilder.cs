using Seller.Web.Areas.Orders.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using System.Globalization;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrderDetailViewModel> orderDetailModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public OrderPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, OrderDetailViewModel> orderDetailModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.orderDetailModelBuilder = orderDetailModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<OrderPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                OrderDetail = await this.orderDetailModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
