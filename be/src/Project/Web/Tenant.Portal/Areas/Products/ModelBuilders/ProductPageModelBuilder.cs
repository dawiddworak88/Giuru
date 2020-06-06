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
using Tenant.Portal.Areas.Products.Repositories;
using Tenant.Portal.Areas.Products.ViewModels;

namespace Tenant.Portal.Areas.Products.ModelBuilders
{
    public class ProductPageModelBuilder : IAsyncComponentModelBuilder<ProductsComponentModel, ProductPageViewModel>
    {
        private readonly IProductsRepository productsRepository;
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ProductPageModelBuilder(
            IProductsRepository productsRepository,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator,
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.productsRepository = productsRepository;
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
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Title = this.productLocalizer["Products"],
                ShowNew = true,
                NewText = this.productLocalizer["NewProduct"],
                NewUrl = this.linkGenerator.GetPathByAction("Index", "ProductDetail", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                Products = await this.productsRepository.GetProductsAsync(componentModel.Token, CultureInfo.CurrentUICulture.Name, null, Foundation.GenericRepository.Definitions.Constants.DefaultPageIndex, Foundation.GenericRepository.Definitions.Constants.DefaultItemsPerPage),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
