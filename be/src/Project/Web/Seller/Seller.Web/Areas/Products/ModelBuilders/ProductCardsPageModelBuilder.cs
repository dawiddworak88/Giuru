using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.ViewModels;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class ProductCardsPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductCardsPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> _headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> _menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ProductCardCategory>> _categoryCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;

        public ProductCardsPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ProductCardCategory>> categoryCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            _headerModelBuilder = headerModelBuilder;
            _menuTilesModelBuilder = menuTilesModelBuilder;
            _categoryCatalogModelBuilder = categoryCatalogModelBuilder;
            _footerModelBuilder = footerModelBuilder;
        }

        public async Task<ProductCardsPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductCardsPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await _headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = _menuTilesModelBuilder.BuildModel(),
                Catalog = await _categoryCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = _footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
