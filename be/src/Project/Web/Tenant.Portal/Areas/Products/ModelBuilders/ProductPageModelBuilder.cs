using Feature.PageContent.Components.Footers.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Tenant.Portal.Areas.Products.ViewModels;

namespace Tenant.Portal.Areas.Products.ModelBuilders
{
    public class ProductPageModelBuilder : IModelBuilder<ProductPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;

        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;

        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        private readonly IModelBuilder<ProductCatalogViewModel> productCatalogModelBuilder;

        public ProductPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IModelBuilder<ProductCatalogViewModel> productCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.productCatalogModelBuilder = productCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public ProductPageViewModel BuildModel()
        {
            var viewModel = new ProductPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Catalog = productCatalogModelBuilder.BuildModel(),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
