using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Outlet.DomainModels;
using Seller.Web.Areas.Outlet.ViewModel;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Outlet.ModelBuilders
{
    public class OutletPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OutletPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<OutletItem>> outletCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public OutletPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<OutletItem>> outletCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.outletCatalogModelBuilder = outletCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<OutletPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OutletPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = this.headerModelBuilder.BuildModel(),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                Catalog = await this.outletCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
