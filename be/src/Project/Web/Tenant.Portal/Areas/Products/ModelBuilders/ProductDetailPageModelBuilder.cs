using Feature.PageContent.Components.Footers.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Tenant.Portal.Areas.Products.ViewModels;

namespace Tenant.Portal.Areas.Products.ModelBuilders
{
    public class ProductDetailPageModelBuilder : IModelBuilder<ProductDetailPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;

        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;

        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public ProductDetailPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public ProductDetailPageViewModel BuildModel()
        {
            var viewModel = new ProductDetailPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
