using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Products.ComponentModels;
using Tenant.Portal.Areas.Products.Repositories;
using Tenant.Portal.Areas.Products.ViewModels;
using Tenant.Portal.Shared.Catalogs.ModelBuilders;

namespace Tenant.Portal.Areas.Products.ModelBuilders
{
    public class ProductPageCatalogModelBuilder : IAsyncComponentModelBuilder<ProductsCatalogComponentModel, ProductPageCatalogViewModel>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IProductsRepository productsRepository;
        private readonly LinkGenerator linkGenerator;

        public ProductPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IProductsRepository productsRepository,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.productsRepository = productsRepository;
            this.linkGenerator = linkGenerator;
        }

        public async Task<ProductPageCatalogViewModel> BuildModelAsync(ProductsCatalogComponentModel componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<ProductPageCatalogViewModel>();

            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.PagedProducts = await this.productsRepository.GetProductsAsync(componentModel.Token, CultureInfo.CurrentUICulture.Name, null, Foundation.GenericRepository.Definitions.Constants.DefaultPageIndex, Foundation.GenericRepository.Definitions.Constants.DefaultItemsPerPage);
            
            return viewModel;
        }
    }
}