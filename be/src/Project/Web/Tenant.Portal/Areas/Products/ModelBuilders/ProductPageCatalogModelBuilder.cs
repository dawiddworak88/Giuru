using Feature.Product;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Products.Repositories;
using Tenant.Portal.Areas.Products.ViewModels;
using Tenant.Portal.Shared.Catalogs.ModelBuilders;
using Tenant.Portal.Shared.ComponentModels;

namespace Tenant.Portal.Areas.Products.ModelBuilders
{
    public class ProductPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductPageCatalogViewModel>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IProductsRepository productsRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ProductPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IProductsRepository productsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.productsRepository = productsRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<ProductPageCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<ProductPageCatalogViewModel>();

            viewModel.SkuLabel = this.productLocalizer["Sku"];
            viewModel.NameLabel = this.globalLocalizer["Name"];
            viewModel.LastModifiedDateLabel = this.globalLocalizer["LastModifiedDate"];
            viewModel.CreatedDateLabel = this.globalLocalizer["CreatedDate"];
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "ProductDetail", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "ProductApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.PagedProducts = await this.productsRepository.GetProductsAsync(componentModel.Token, CultureInfo.CurrentUICulture.Name, null, Foundation.GenericRepository.Definitions.Constants.DefaultPageIndex, Foundation.GenericRepository.Definitions.Constants.DefaultItemsPerPage);
            
            return viewModel;
        }
    }
}