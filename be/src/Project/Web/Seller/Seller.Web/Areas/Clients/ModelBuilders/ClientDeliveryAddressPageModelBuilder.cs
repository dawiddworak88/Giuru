using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientDeliveryAddressPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientDeliveryAddressPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> _headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> _menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientDeliveryAddressFormViewModel> _clientDeliveryAddressFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;

        public ClientDeliveryAddressPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ClientDeliveryAddressFormViewModel> clientDeliveryAddressFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _menuTilesModelBuilder = menuTilesModelBuilder;
            _clientDeliveryAddressFormModelBuilder = clientDeliveryAddressFormModelBuilder;
            _footerModelBuilder = footerModelBuilder;
        }

        public async Task<ClientDeliveryAddressPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientDeliveryAddressPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = _menuTilesModelBuilder.BuildModel(),
                ClientDeliveryAddressForm = await _clientDeliveryAddressFormModelBuilder.BuildModelAsync(componentModel),
                Footer = _footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
