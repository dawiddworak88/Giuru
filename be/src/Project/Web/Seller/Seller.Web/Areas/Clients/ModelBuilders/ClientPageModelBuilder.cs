using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using System.Threading.Tasks;
using Seller.Web.Areas.Clients.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Globalization;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientFormViewModel> clientFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public ClientPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ClientFormViewModel> clientFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.clientFormModelBuilder = clientFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<ClientPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                ClientForm = await clientFormModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}