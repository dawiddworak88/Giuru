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
    public class ProductPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductPageViewModel>
    {
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductFormViewModel> productFormModelBuilder;

        public ProductPageModelBuilder(
            IStringLocalizer<ProductResources> productLocalizer,
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ProductFormViewModel> productFormModelBuilder)
        {
            this.productLocalizer = productLocalizer;
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.productFormModelBuilder = productFormModelBuilder;
        }

        public async Task<ProductPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductPageViewModel
            {
                Title = this.productLocalizer["Product"],
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Footer = footerModelBuilder.BuildModel(),
                ProductForm = await productFormModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}
