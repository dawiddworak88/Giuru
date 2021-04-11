using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Shared.ModelBuilders.Breadcrumbs;
using Buyer.Web.Shared.ViewModels.Breadcrumbs;
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

                viewModel.Items.Add(categoryBreadcrumb);

                if (product.PrimaryProductId.HasValue)
                {
                    var primaryProduct = await this.productsRepository.GetProductAsync(product.PrimaryProductId.Value, componentModel.Language, componentModel.Token);

                    if (primaryProduct != null)
                    {
                        var primaryProductBreadcrumb = new BreadcrumbViewModel
                        {
                            Name = primaryProduct.Name,
                            Url = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, primaryProduct.Id }),
                            IsActive = false
                        };

                        var secondaryProductBreadcrumb = new BreadcrumbViewModel
                        {
                            Name = $"{product.Name} ({product.Sku})",
                            Url = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, product.Id }),
                            IsActive = true
                        };

                        viewModel.Items.Add(primaryProductBreadcrumb);
                        viewModel.Items.Add(secondaryProductBreadcrumb);
                    }
                }
                else
                {
                    var productBreadcrumb = new BreadcrumbViewModel
                    {
                        Name = product.Name,
                        Url = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, product.Id }),
                        IsActive = true
                    };

                    viewModel.Items.Add(productBreadcrumb);
                }
            }

            return viewModel;
        }
    }
}
