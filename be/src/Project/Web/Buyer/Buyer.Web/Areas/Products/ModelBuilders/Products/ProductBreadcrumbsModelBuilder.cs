using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Shared.Breadcrumbs.ModelBuilders;
using Buyer.Web.Shared.Breadcrumbs.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Products
{
    public class ProductBreadcrumbsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductBreadcrumbsViewModel>
    {
        private readonly LinkGenerator linkGenerator;
        private readonly IProductsRepository productsRepository;
        private readonly IBreadcrumbsModelBuilder<ComponentModelBase, ProductBreadcrumbsViewModel> breadcrumbsModelBuilder;

        public ProductBreadcrumbsModelBuilder(
            LinkGenerator linkGenerator,
            IProductsRepository productsRepository,
            IBreadcrumbsModelBuilder<ComponentModelBase, ProductBreadcrumbsViewModel> breadcrumbsModelBuilder)
        {
            this.linkGenerator = linkGenerator;
            this.breadcrumbsModelBuilder = breadcrumbsModelBuilder;
            this.productsRepository = productsRepository;
        }

        public async Task<ProductBreadcrumbsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.breadcrumbsModelBuilder.BuildModel(componentModel);

            var product = await this.productsRepository.GetProductAsync(componentModel.Id, componentModel.Language, componentModel.Token);

            if (product != null)
            {
                var categoryBreadcrumb = new BreadcrumbViewModel
                {
                    Name = product.CategoryName,
                    Url = this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = product.CategoryId }),
                    IsActive = false
                };

                var productBreadcrumb = new BreadcrumbViewModel
                { 
                    Name = product.Name,
                    Url = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = product.Id }),
                    IsActive = true
                };

                viewModel.Items.Add(categoryBreadcrumb);
                viewModel.Items.Add(productBreadcrumb);
            }

            return viewModel;
        }
    }
}
