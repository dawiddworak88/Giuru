using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ModelBuilders.Definitions;
using Buyer.Web.Areas.Products.Repositories.Categories;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Buyer.Web.Shared.Catalogs.ViewModels;
using Buyer.Web.Shared.Definitions;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Categories
{
    public class CategoryCatalogModelBuilder : IAsyncComponentModelBuilder<CategoryComponentModel, CategoryCatalogViewModel>
    {
        private readonly IProductsService productsService;
        private readonly ICategoryRepository categoryRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public CategoryCatalogModelBuilder(
            IProductsService productsService,
            ICategoryRepository categoryRepository,
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.productsService = productsService;
            this.categoryRepository = categoryRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CategoryCatalogViewModel> BuildModelAsync(CategoryComponentModel componentModel)
        {
            var viewModel = new CategoryCatalogViewModel
            {
                SkuLabel = this.productLocalizer.GetString("Sku"),
                SignInUrl = "#",
                SignInToSeePricesLabel = this.globalLocalizer.GetString("SignInToSeePrices"),
                ResultsLabel = this.globalLocalizer.GetString("Results"),
                ByLabel = this.globalLocalizer.GetString("By"),
                InStockLabel = this.productLocalizer.GetString("InStock"),
                NoResultsLabel = this.globalLocalizer.GetString("NoResults"),
                GeneralErrorMessage = this.globalLocalizer["AnErrorOccurred"],
                DisplayedRowsLabel = this.globalLocalizer["DisplayedRows"],
                RowsPerPageLabel = this.globalLocalizer["RowsPerPage"],
                BackIconButtonText = this.globalLocalizer["Previous"],
                NextIconButtonText = this.globalLocalizer["Next"],
                IsAuthenticated = componentModel.IsAuthenticated,
                ProductsApiUrl = this.linkGenerator.GetPathByAction("Get", "ProductsApi", new { Area = "Products" }),
                PagedItems = await this.productsService.GetProductsAsync(
                    componentModel.Id,
                    componentModel.Language,
                    componentModel.SearchTerm,
                    PaginationConstants.DefaultPageIndex,
                    CategoryConstants.CategoryCatalogPaginationPageSize,
                    componentModel.Token)
            };

            var category = await this.categoryRepository.GetCategoryAsync(componentModel.Id, componentModel.Token);

            if (category != null)
            {
                viewModel.Title = category.Name;
                viewModel.Id = category.Id;
            }

            return viewModel;
        }
    }
}
