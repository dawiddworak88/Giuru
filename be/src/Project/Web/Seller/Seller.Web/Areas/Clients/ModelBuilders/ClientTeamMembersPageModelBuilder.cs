using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Clients.ComponentModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientTeamMembersPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientTeamMembersPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> _headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> _menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientTeamMember>> _catalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;

        public ClientTeamMembersPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientTeamMember>> catalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _menuTilesModelBuilder = menuTilesModelBuilder;
            _catalogModelBuilder = catalogModelBuilder;
            _footerModelBuilder = footerModelBuilder;
        }

        public async Task<ClientTeamMembersPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientTeamMembersPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = _menuTilesModelBuilder.BuildModel(),
                Catalog = await _catalogModelBuilder.BuildModelAsync(componentModel),
                Footer = _footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
