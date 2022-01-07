using Buyer.Web.Areas.Products.Repositories.Categories;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Buyer.Web.Shared.ModelBuilders.Breadcrumbs;
using Buyer.Web.Shared.ViewModels.Breadcrumbs;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Categories
{
    public class CategoryBreadcrumbsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoryBreadcrumbsViewModel>
    {
        private readonly LinkGenerator linkGenerator;
        private readonly ICategoryRepository categoryRepository;
        private readonly IBreadcrumbsModelBuilder<ComponentModelBase, CategoryBreadcrumbsViewModel> breadcrumbsModelBuilder;

        public CategoryBreadcrumbsModelBuilder(
            LinkGenerator linkGenerator,
            ICategoryRepository categoryRepository,
            IBreadcrumbsModelBuilder<ComponentModelBase, CategoryBreadcrumbsViewModel> breadcrumbsModelBuilder)
        {
            this.linkGenerator = linkGenerator;
            this.breadcrumbsModelBuilder = breadcrumbsModelBuilder;
            this.categoryRepository = categoryRepository;
        }

        public async Task<CategoryBreadcrumbsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.breadcrumbsModelBuilder.BuildModel(componentModel);

            var category = await this.categoryRepository.GetCategoryAsync(componentModel.Id, componentModel.Token, componentModel.Language);

            if (category != null)
            {
                var categoryBreadcrumb = new BreadcrumbViewModel
                {
                    Name = category.Name,
                    Url = this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = category.Id }),
                    IsActive = true
                };

                viewModel.Items.Add(categoryBreadcrumb);
            }

            return viewModel;
        }
    }
}
