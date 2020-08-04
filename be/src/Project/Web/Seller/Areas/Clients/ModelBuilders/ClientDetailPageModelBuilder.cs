using Feature.Client;
using Feature.PageContent.Components.Footers.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ViewModels;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientDetailPageModelBuilder : IModelBuilder<ClientDetailPageViewModel>
    {
        private readonly IStringLocalizer<ClientResources> clientLocalizer;

        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;

        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;

        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        private readonly IModelBuilder<ClientDetailFormViewModel> clientDetailFormModelBuilder;

        public ClientDetailPageModelBuilder(
            IStringLocalizer<ClientResources> clientLocalizer,
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IModelBuilder<ClientDetailFormViewModel> clientDetailFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.clientLocalizer = clientLocalizer;
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.clientDetailFormModelBuilder = clientDetailFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public ClientDetailPageViewModel BuildModel()
        {
            var viewModel = new ClientDetailPageViewModel
            {
                Title = clientLocalizer["Client"],
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                ClientDetailForm = clientDetailFormModelBuilder.BuildModel(),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
