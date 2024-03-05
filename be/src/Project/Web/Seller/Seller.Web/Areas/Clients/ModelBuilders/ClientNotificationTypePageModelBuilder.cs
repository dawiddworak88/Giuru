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
    public class ClientNotificationTypePageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientNotificationTypePageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> _headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> _menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientNotificationTypeFormViewModel> _clientNotificationTypeFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;

        public ClientNotificationTypePageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ClientNotificationTypeFormViewModel> clientNotificationTypeFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _menuTilesModelBuilder = menuTilesModelBuilder;
            _clientNotificationTypeFormModelBuilder = clientNotificationTypeFormModelBuilder;
            _footerModelBuilder = footerModelBuilder;
        }

        public async Task<ClientNotificationTypePageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientNotificationTypePageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = _menuTilesModelBuilder.BuildModel(),
                NotificationTypeForm = await _clientNotificationTypeFormModelBuilder.BuildModelAsync(componentModel),
                Footer = _footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
