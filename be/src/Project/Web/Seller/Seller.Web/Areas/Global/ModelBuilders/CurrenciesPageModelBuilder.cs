using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Global.DomainModels;
using Seller.Web.Areas.Global.ViewModels;
using Seller.Web.Shared.ViewModels;
using System;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Global.ModelBuilders
{
    public class CurrenciesPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CurrenciesPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> _headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> _menuTitlesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Currency>> _currencyCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;

        public CurrenciesPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTitlesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Currency>> currencyCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _menuTitlesModelBuilder = menuTitlesModelBuilder;
            _currencyCatalogModelBuilder = currencyCatalogModelBuilder;
            _footerModelBuilder = footerModelBuilder;
        }

        public async Task<CurrenciesPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new CurrenciesPageViewModel
            {
                Locale = CultureInfo.CurrentCulture.Name,
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = _menuTitlesModelBuilder.BuildModel(),
                Catalog = await _currencyCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = _footerModelBuilder.BuildModel()
            };

            Console.WriteLine("viewModel " + JsonSerializer.Serialize(viewModel.Catalog));

            return viewModel;
        }
    }
}