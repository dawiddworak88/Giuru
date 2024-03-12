using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.ViewModel;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderAttributePageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderAttributePageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> _headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> _menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrderAttributeFormViewModel> _orderAttributeFormModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<OrderAttributeOption>> _orderAttributeOptionsCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;

        public OrderAttributePageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder, 
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder, 
            IAsyncComponentModelBuilder<ComponentModelBase, OrderAttributeFormViewModel> orderAttributeFormModelBuilder, 
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<OrderAttributeOption>> orderAttributeOptionsCatalogModelBuilder, 
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _menuTilesModelBuilder = menuTilesModelBuilder;
            _orderAttributeFormModelBuilder = orderAttributeFormModelBuilder;
            _orderAttributeOptionsCatalogModelBuilder = orderAttributeOptionsCatalogModelBuilder;
            _footerModelBuilder = footerModelBuilder;
        }

        public async Task<OrderAttributePageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderAttributePageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = _menuTilesModelBuilder.BuildModel(),
                OrderAttributeForm = await _orderAttributeFormModelBuilder.BuildModelAsync(componentModel),
                Footer = _footerModelBuilder.BuildModel()
            };

            if (componentModel.Id.HasValue)
            {
                viewModel.Id = componentModel.Id;
                //viewModel.Type =
            }

            return viewModel;
        }
    }
}
