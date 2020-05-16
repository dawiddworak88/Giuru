using Feature.PageContent.Components.Footers.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.MenuTiles.ViewModels;
using Feature.Product.Resources;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using Tenant.Portal.Areas.Products.ViewModels;

namespace Tenant.Portal.Areas.Products.ModelBuilders
{
    public class ProductDetailPageModelBuilder : IModelBuilder<ProductDetailPageViewModel>
    {
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IModelBuilder<ProductDetailFormViewModel> productDetailFormModelBuilder;

        public ProductDetailPageModelBuilder(
            IStringLocalizer<ProductResources> productLocalizer,
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IModelBuilder<ProductDetailFormViewModel> productDetailFormModelBuilder)
        {
            this.productLocalizer = productLocalizer;
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.productDetailFormModelBuilder = productDetailFormModelBuilder;
        }

        public ProductDetailPageViewModel BuildModel()
        {
            var viewModel = new ProductDetailPageViewModel
            {
                Title = this.productLocalizer["Product"],
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Footer = footerModelBuilder.BuildModel(),
                ProductDetailForm = productDetailFormModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
