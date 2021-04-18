using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.ViewModels;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class ProductAttributePageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductAttributePageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductAttributeFormViewModel> productAttributeFormModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ProductAttributeItem>> productAttributeItemsCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public ProductAttributePageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ProductAttributeFormViewModel> productAttributeFormModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ProductAttributeItem>> productAttributeItemsCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.productAttributeFormModelBuilder = productAttributeFormModelBuilder;
            this.productAttributeItemsCatalogModelBuilder = productAttributeItemsCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<ProductAttributePageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductAttributePageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Id = componentModel.Id,
                Header = this.headerModelBuilder.BuildModel(),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                ProductAttributeForm = await this.productAttributeFormModelBuilder.BuildModelAsync(componentModel),
                Catalog = componentModel.Id.HasValue ? await this.productAttributeItemsCatalogModelBuilder.BuildModelAsync(componentModel) : null,
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
