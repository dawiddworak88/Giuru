using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Global.ViewModels;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Global.ModelBuilders
{
    public class CountryPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CountryPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CountryFormViewModel> countryFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public CountryPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CountryFormViewModel> countryFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.countryFormModelBuilder = countryFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<CountryPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new CountryPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                CountryForm = await this.countryFormModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
