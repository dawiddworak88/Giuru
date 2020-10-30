using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ViewModels;
using Foundation.PageContent.ComponentModels;
using Foundation.Localization;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class ProductDetailPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailPageViewModel>
    {
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailFormViewModel> productDetailFormModelBuilder;

        public ProductDetailPageModelBuilder(
            IStringLocalizer<ProductResources> productLocalizer,
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailFormViewModel> productDetailFormModelBuilder)
        {
            this.productLocalizer = productLocalizer;
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.productDetailFormModelBuilder = productDetailFormModelBuilder;
        }

        public async Task<ProductDetailPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var productDetailFormComponentModel = new ComponentModelBase
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
