using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Brands;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Brands
{
    public class BrandCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, BrandCatalogViewModel>
    {
        private readonly ICatalogModelBuilder<ComponentModelBase, BrandCatalogViewModel> catalogModelBuilder;
        private readonly IProductsService productsService;
        private readonly IStringLocalizer<ProductResources> productLocalizer;

        public BrandCatalogModelBuilder(
            ICatalogModelBuilder<ComponentModelBase, BrandCatalogViewModel> catalogModelBuilder,
            IProductsService productsService,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.productsService = productsService;
            this.productLocalizer = productLocalizer;
        }

        public async Task<BrandCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel(componentModel);

            viewModel.BrandId = componentModel.Id;

            viewModel.Title = this.productLocalizer.GetString("Products");

            viewModel.PagedItems = await this.productsService.GetProductsAsync(
                null,
                componentModel.Id,
                componentModel.Language,
                null,
                PaginationConstants.DefaultPageIndex,
                ProductConstants.ProductsCatalogPaginationPageSize,
                componentModel.Token);

            return viewModel;
        }
    }
}
