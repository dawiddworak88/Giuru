using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ViewModels;
using Seller.Web.Areas.Products.ComponentModels;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class DuplicateProductPageModelBuilder : IAsyncComponentModelBuilder<DuplicateProductComponentModel, ProductPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<DuplicateProductComponentModel, ProductFormViewModel> duplicateProductFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public DuplicateProductPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, SellerHeaderViewModel> headerModelBuilder,
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
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                ProductForm = await this.duplicateProductFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
