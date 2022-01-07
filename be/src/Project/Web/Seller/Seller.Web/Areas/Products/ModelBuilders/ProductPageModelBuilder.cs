using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ViewModels;
using Foundation.PageContent.ComponentModels;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class ProductPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductFormViewModel> productFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public ProductPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ProductFormViewModel> productFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.productFormModelBuilder = productFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<ProductPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductPageViewModel
            {
                Header = this.headerModelBuilder.BuildModel(),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                ProductForm = await this.productFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
