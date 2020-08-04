using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Feature.Product;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ViewModels;
using Seller.Web.Shared.ComponentModels;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class ProductPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductPageCatalogViewModel> productCatalogModelBuilder;
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ProductPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, ProductPageCatalogViewModel> productCatalogModelBuilder,
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

        public async Task<ProductPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Title = this.productLocalizer["Products"],
                NewText = this.productLocalizer["NewProduct"],
                NewUrl = this.linkGenerator.GetPathByAction("Edit", "ProductDetail", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                Catalog = await this.productCatalogModelBuilder.BuildModelAsync(new ComponentModelBase { Token = componentModel.Token }),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
