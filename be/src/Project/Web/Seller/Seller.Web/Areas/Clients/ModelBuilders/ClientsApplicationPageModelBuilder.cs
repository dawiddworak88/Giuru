using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Seller.Web.Areas.Clients.ViewModels;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Shared.ViewModels;
using Seller.Web.Areas.Clients.DomainModels;
using System.Threading.Tasks;
using System.Globalization;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientsApplicationPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientsApplicationPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientApplication>> clientsApplicationCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public ClientsApplicationPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientApplication>> clientsApplicationCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.clientsApplicationCatalogModelBuilder = clientsApplicationCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<ClientsApplicationPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientsApplicationPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Catalog = await this.clientsApplicationCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}