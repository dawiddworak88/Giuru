using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Buyer.Web.Shared.Catalogs.ViewModels;
using Buyer.Web.Shared.Definitions;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Categories
{
    public class CategoryCatalogModelBuilder : IAsyncComponentModelBuilder<CategoryComponentModel, CategoryCatalogViewModel>
    {
        private readonly IProductsRepository productsRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;

        public CategoryCatalogModelBuilder(
            IProductsRepository productsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.productsRepository = productsRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
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
                IsAuthenticated = componentModel.IsAuthenticated
            };

            var pagedProducts = await this.productsRepository.GetProductsAsync(
                componentModel.CategoryId,
                componentModel.Language,
                componentModel.SearchTerm,
                PaginationConstants.DefaultPageIndex,
                PaginationConstants.DefaultPageSize,
                componentModel.Token);

            if (pagedProducts?.Data != null)
            {
                var catalogItemList = new List<CatalogItemViewModel>();

                foreach (var product in pagedProducts.Data)
                {
                    var catalogItem = new CatalogItemViewModel
                    { 
                         Id = product.Id,
                         Sku = product.Sku,
                         Title = product.Name
                    };

                    catalogItemList.Add(catalogItem);
                }

                viewModel.Items = new PagedResults<IEnumerable<CatalogItemViewModel>>(pagedProducts.Total, pagedProducts.PageSize)
                { 
                    Data = catalogItemList
                };
            }

            return viewModel;
        }
    }
}
