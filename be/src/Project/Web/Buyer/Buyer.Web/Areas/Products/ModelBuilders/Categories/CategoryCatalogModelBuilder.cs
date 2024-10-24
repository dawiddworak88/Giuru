﻿using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Repositories.Categories;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using System.Threading.Tasks;
using Foundation.PageContent.ComponentModels;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Buyer.Web.Shared.ViewModels.Modals;
using Buyer.Web.Areas.Products.DomainModels;

namespace Buyer.Web.Areas.Products.ModelBuilders.Categories
{
    public class CategoryCatalogModelBuilder : IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel>
    {
        private readonly ICatalogModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel> catalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder;
        private readonly IProductsService productsService;
        private readonly ICategoryRepository categoryRepository;

        public CategoryCatalogModelBuilder(
            ICatalogModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel> catalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder,
            IProductsService productsService,
            ICategoryRepository categoryRepository)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.sidebarModelBuilder = sidebarModelBuilder;
            this.productsService = productsService;
            this.categoryRepository = categoryRepository;
            this.modalModelBuilder = modalModelBuilder;
        }

        public async Task<CategoryCatalogViewModel> BuildModelAsync(SearchProductsComponentModel componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel(componentModel);

            var category = await this.categoryRepository.GetCategoryAsync(componentModel.Id, componentModel.Token, componentModel.Language);

            if (category != null)
            {
                viewModel.Title = category.Name;
                viewModel.CategoryId = category.Id;
                viewModel.Sidebar = await this.sidebarModelBuilder.BuildModelAsync(componentModel);
                viewModel.Modal = await this.modalModelBuilder.BuildModelAsync(componentModel);
                viewModel.OrderBy = nameof(Product.Name);
                viewModel.PagedItems = await this.productsService.GetProductsAsync(
                    null,
                    componentModel.Id,
                    null,
                    componentModel.Language,
                    componentModel.SearchTerm,
                    false,
                    PaginationConstants.DefaultPageIndex,
                    ProductConstants.ProductsCatalogPaginationPageSize,
                    componentModel.Token);
            }

            return viewModel;
        }
    }
}
