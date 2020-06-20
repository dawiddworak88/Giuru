using Feature.PageContent.Components.Footers.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.MenuTiles.ViewModels;
using Feature.Product;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Products.ComponentModels;
using Tenant.Portal.Areas.Products.ViewModels;

namespace Tenant.Portal.Areas.Products.ModelBuilders
{
    public class ProductDetailPageModelBuilder : IAsyncComponentModelBuilder<ProductDetailComponentModel, ProductDetailPageViewModel>
    {
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ProductDetailFormComponentModel, ProductDetailFormViewModel> productDetailFormModelBuilder;

        public ProductDetailPageModelBuilder(
            IStringLocalizer<ProductResources> productLocalizer,
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IAsyncComponentModelBuilder<ProductDetailFormComponentModel, ProductDetailFormViewModel> productDetailFormModelBuilder)
        {
            this.productLocalizer = productLocalizer;
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.productDetailFormModelBuilder = productDetailFormModelBuilder;
        }

        public async Task<ProductDetailPageViewModel> BuildModelAsync(ProductDetailComponentModel componentModel)
        {
            var productDetailFormComponentModel = new ProductDetailFormComponentModel
            { 
                Id = componentModel.Id,
                Token = componentModel.Token,
                Language = componentModel.Language
            };

            var viewModel = new ProductDetailPageViewModel
            {
                Title = this.productLocalizer["Product"],
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Footer = footerModelBuilder.BuildModel(),
                ProductDetailForm = await productDetailFormModelBuilder.BuildModelAsync(productDetailFormComponentModel)
            };

            return viewModel;
        }
    }
}
