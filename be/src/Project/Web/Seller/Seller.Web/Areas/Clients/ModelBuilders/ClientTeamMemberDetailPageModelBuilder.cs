using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Clients.ComponentModels;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientTeamMemberDetailPageModelBuilder : IAsyncComponentModelBuilder<ClientTeamMemberComponentModel, ClientTeamMemberDetailPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> _headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> _menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ClientTeamMemberComponentModel, ClientTeamMemberFormViewModel> _formModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;

        public ClientTeamMemberDetailPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ClientTeamMemberComponentModel, ClientTeamMemberFormViewModel> formModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _menuTilesModelBuilder = menuTilesModelBuilder;
            _formModelBuilder = formModelBuilder;
            _footerModelBuilder = footerModelBuilder;
        }

        public async Task<ClientTeamMemberDetailPageViewModel> BuildModelAsync(ClientTeamMemberComponentModel componentModel)
        {
            var viewModel = new ClientTeamMemberDetailPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = _menuTilesModelBuilder.BuildModel(),
                ClientTeamMemberForm = await _formModelBuilder.BuildModelAsync(componentModel),
                Footer = _footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
