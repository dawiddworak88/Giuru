using Feature.Client;
using Feature.PageContent.Components.Footers.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Tenant.Portal.Areas.Clients.ViewModels;

namespace Tenant.Portal.Areas.Clients.ModelBuilders
{
    public class ClientPageModelBuilder : IModelBuilder<ClientPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ClientPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IStringLocalizer<ClientResources> clientLocalizer,
            LinkGenerator linkGenerator,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.clientLocalizer = clientLocalizer;
            this.linkGenerator = linkGenerator;
            this.footerModelBuilder = footerModelBuilder;
        }

        public ClientPageViewModel BuildModel()
        {
            var viewModel = new ClientPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Title = this.clientLocalizer["Clients"],
                ShowNew = true,
                NewText = this.clientLocalizer["NewClient"],
                NewUrl = this.linkGenerator.GetPathByAction("Index", "ClientDetail", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
