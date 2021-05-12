using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ViewModels;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Areas.Products.ComponentModels;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class DuplicateProductPageModelBuilder : IAsyncComponentModelBuilder<DuplicateProductComponentModel, ProductPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<DuplicateProductComponentModel, ProductFormViewModel> duplicateProductFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public DuplicateProductPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<DuplicateProductComponentModel, ProductFormViewModel> productFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.duplicateProductFormModelBuilder = productFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<ProductPageViewModel> BuildModelAsync(DuplicateProductComponentModel componentModel)
        {
            var viewModel = new ProductPageViewModel
            {
                Header = this.headerModelBuilder.BuildModel(),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                ProductForm = await this.duplicateProductFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
