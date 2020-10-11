using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Brands;
using Buyer.Web.Shared.Catalogs.ModelBuilders;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Brands
{
    public class BrandCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, BrandCatalogViewModel>
    {
        private readonly ICatalogModelBuilder<ComponentModelBase, BrandCatalogViewModel> catalogModelBuilder;
        private readonly IProductsService productsService;

        public BrandCatalogModelBuilder(
            ICatalogModelBuilder<ComponentModelBase, BrandCatalogViewModel> catalogModelBuilder,
            IProductsService productsService)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.productsService = productsService;
        }

        public async Task<BrandCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel(componentModel);
            return viewModel;
        }
    }
}
