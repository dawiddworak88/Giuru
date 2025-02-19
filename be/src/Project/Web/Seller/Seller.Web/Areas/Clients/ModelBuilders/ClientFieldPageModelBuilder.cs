﻿using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientFieldPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientFieldPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> _headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> _menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientFieldFormViewModel> _clientFieldFormModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientFieldOption>> _clientFieldOptionsCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;

        public ClientFieldPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ClientFieldFormViewModel> clientFieldFormModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientFieldOption>> clientFieldOptionsCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _menuTilesModelBuilder = menuTilesModelBuilder;
            _clientFieldFormModelBuilder = clientFieldFormModelBuilder;
            _clientFieldOptionsCatalogModelBuilder = clientFieldOptionsCatalogModelBuilder;
            _footerModelBuilder = footerModelBuilder;
        }

        public async Task<ClientFieldPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientFieldPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = _menuTilesModelBuilder.BuildModel(),
                ClientFieldForm = await _clientFieldFormModelBuilder.BuildModelAsync(componentModel),
                Footer = _footerModelBuilder.BuildModel()
            };

            var fieldType = viewModel.ClientFieldForm.Type;

            if (componentModel.Id.HasValue && fieldType == "select")
            {
                viewModel.Id = componentModel.Id;
                viewModel.FieldType = fieldType;
                viewModel.Catalog = await _clientFieldOptionsCatalogModelBuilder.BuildModelAsync(componentModel);
            }

            return viewModel;
        }
    }
}
