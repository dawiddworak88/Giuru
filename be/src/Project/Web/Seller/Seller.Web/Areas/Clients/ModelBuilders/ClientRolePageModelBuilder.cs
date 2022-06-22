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
    public class ClientRolePageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientRolePageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientRoleFormViewModel> roleFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public ClientRolePageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ClientRoleFormViewModel> roleFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.roleFormModelBuilder = roleFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<ClientRolePageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientRolePageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                ClientRoleForm = await this.roleFormModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}