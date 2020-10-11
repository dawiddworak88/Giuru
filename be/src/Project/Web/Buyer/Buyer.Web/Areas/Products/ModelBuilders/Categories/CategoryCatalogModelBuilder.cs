using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ModelBuilders.Definitions;
using Buyer.Web.Areas.Products.Repositories.Categories;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Buyer.Web.Shared.Catalogs.ModelBuilders;
using Buyer.Web.Shared.Definitions;
using Foundation.Extensions.ModelBuilders;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Categories
{
    public class CategoryCatalogModelBuilder : IAsyncComponentModelBuilder<CategoryComponentModel, CategoryCatalogViewModel>
    {
        private readonly ICatalogModelBuilder<CategoryComponentModel, CategoryCatalogViewModel> catalogModelBuilder;
        private readonly IProductsService productsService;
        private readonly ICategoryRepository categoryRepository;

        public CategoryCatalogModelBuilder(
            ICatalogModelBuilder<CategoryComponentModel, CategoryCatalogViewModel> catalogModelBuilder,
            IProductsService productsService,
            ICategoryRepository categoryRepository)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.productsService = productsService;
            this.categoryRepository = categoryRepository;
        }

        public async Task<CategoryCatalogViewModel> BuildModelAsync(CategoryComponentModel componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel(componentModel);
            
            viewModel.PagedItems = await this.productsService.GetProductsAsync(
                componentModel.Id,
                componentModel.Language,
                componentModel.SearchTerm,
                PaginationConstants.DefaultPageIndex,
                CategoryConstants.CategoryCatalogPaginationPageSize,
                componentModel.Token);

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
