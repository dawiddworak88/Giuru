using Feature.PageContent.Components.Footers.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.MenuTiles.ViewModels;
using Feature.Product.Resources;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Products.ComponentModels;
using Tenant.Portal.Areas.Products.ViewModels;

namespace Tenant.Portal.Areas.Products.ModelBuilders
{
    public class ProductPageModelBuilder : IAsyncComponentModelBuilder<ProductsComponentModel, ProductPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ProductsCatalogComponentModel, ProductPageCatalogViewModel> productCatalogModelBuilder;
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ProductPageModelBuilder(
            IAsyncComponentModelBuilder<ProductsCatalogComponentModel, ProductPageCatalogViewModel> productCatalogModelBuilder,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator,
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.productCatalogModelBuilder = productCatalogModelBuilder;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<ProductPageViewModel> BuildModelAsync(ProductsComponentModel componentModel)
        {
            var viewModel = new ProductPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Title = this.productLocalizer["Products"],
                NewText = this.productLocalizer["NewProduct"],
                NewUrl = this.linkGenerator.GetPathByAction("Edit", "ProductDetail", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                Catalog = await this.productCatalogModelBuilder.BuildModelAsync(new ProductsCatalogComponentModel { Token = componentModel.Token }),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
