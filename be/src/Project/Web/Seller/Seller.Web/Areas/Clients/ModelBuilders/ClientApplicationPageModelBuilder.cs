using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientApplicationPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientApplicationPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientApplicationFormViewModel> clientApplicationFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public ClientApplicationPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ClientApplicationFormViewModel> clientApplicationFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.clientApplicationFormModelBuilder = clientApplicationFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<ClientApplicationPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientApplicationPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                ClientApplicationForm = await this.clientApplicationFormModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}