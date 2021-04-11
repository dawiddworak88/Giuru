using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Products.ComponentModels;
using Seller.Web.Areas.Products.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class ProductAttributeItemPageModelBuilder : IAsyncComponentModelBuilder<ProductAttributeItemComponentModel, ProductAttributeItemPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ProductAttributeItemComponentModel, ProductAttributeItemFormViewModel> productAttributeItemFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public ProductAttributeItemPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ProductAttributeItemComponentModel, ProductAttributeItemFormViewModel> productAttributeItemFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.productAttributeItemFormModelBuilder = productAttributeItemFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<ProductAttributeItemPageViewModel> BuildModelAsync(ProductAttributeItemComponentModel componentModel)
        {
            var viewModel = new ProductAttributeItemPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = this.headerModelBuilder.BuildModel(),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                ProductAttributeItemForm = await this.productAttributeItemFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
