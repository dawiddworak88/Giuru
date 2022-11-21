using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Global.DomainModels;
using Seller.Web.Areas.Global.ViewModels;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Global.ModelBuilders
{
    public class CountriesPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CountriesPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Country>> countryCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public CountriesPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Country>> countryCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.countryCatalogModelBuilder = countryCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<CountriesPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new CountriesPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Catalog = await this.countryCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
