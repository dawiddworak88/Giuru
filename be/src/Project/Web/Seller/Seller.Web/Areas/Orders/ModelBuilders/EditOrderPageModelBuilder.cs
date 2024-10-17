using Seller.Web.Areas.Orders.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using System.Globalization;
using Seller.Web.Shared.ViewModels;
using Seller.Web.Areas.Orders.ComponetModels;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class EditOrderPageModelBuilder : IAsyncComponentModelBuilder<OrdersPageComponentModel, EditOrderPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> _headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> _menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<OrdersPageComponentModel, EditOrderFormViewModel> _editOrderFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;

        public EditOrderPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<OrdersPageComponentModel, EditOrderFormViewModel> editOrderFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _menuTilesModelBuilder = menuTilesModelBuilder;
            _editOrderFormModelBuilder = editOrderFormModelBuilder;
            _footerModelBuilder = footerModelBuilder;
        }

        public async Task<EditOrderPageViewModel> BuildModelAsync(OrdersPageComponentModel componentModel)
        {
            var viewModel = new EditOrderPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = _menuTilesModelBuilder.BuildModel(),
                EditOrderForm = await _editOrderFormModelBuilder.BuildModelAsync(componentModel),
                Footer = _footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
