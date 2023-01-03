using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientRolesPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientRolesPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientRole>> rolesCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public ClientRolesPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientRole>> rolesCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.rolesCatalogModelBuilder = rolesCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<ClientRolesPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientRolesPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Catalog = await this.rolesCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}