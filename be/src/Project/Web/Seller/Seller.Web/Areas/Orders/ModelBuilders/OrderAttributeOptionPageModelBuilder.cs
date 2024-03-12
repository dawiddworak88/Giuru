using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Orders.ComponentModels;
using Seller.Web.Areas.Orders.ViewModel;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderAttributeOptionPageModelBuilder : IAsyncComponentModelBuilder<OrderAttributeOptionComponentModel, OrderAttributeOptionPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> _headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> _menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<OrderAttributeOptionComponentModel, OrderAttributeOptionFormViewModel> _orderAttributeOptionFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;

        public OrderAttributeOptionPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder, 
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder, 
            IAsyncComponentModelBuilder<OrderAttributeOptionComponentModel, OrderAttributeOptionFormViewModel> orderAttributeOptionFormModelBuilder, 
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _menuTilesModelBuilder = menuTilesModelBuilder;
            _orderAttributeOptionFormModelBuilder = orderAttributeOptionFormModelBuilder;
            _footerModelBuilder = footerModelBuilder;
        }

        public async Task<OrderAttributeOptionPageViewModel> BuildModelAsync(OrderAttributeOptionComponentModel componentModel)
        {
            var viewModel = new OrderAttributeOptionPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = _menuTilesModelBuilder.BuildModel(),
                OrderAttributeOptionForm = await _orderAttributeOptionFormModelBuilder.BuildModelAsync(componentModel),
                Footer = _footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
