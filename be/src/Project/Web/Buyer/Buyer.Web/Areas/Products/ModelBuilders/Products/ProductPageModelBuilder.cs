using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Products
{
    public class ProductPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductBreadcrumbsViewModel> productBreadcrumbsModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailViewModel> productDetailModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder;

        public ProductPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ProductBreadcrumbsViewModel> productBreadcrumbsModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailViewModel> productDetailModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.mainNavigationModelBuilder = mainNavigationModelBuilder;
            this.productBreadcrumbsModelBuilder = productBreadcrumbsModelBuilder;
            this.productDetailModelBuilder = productDetailModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<ProductPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {

            var viewModel = new ProductPageViewModel
            {
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await this.mainNavigationModelBuilder.BuildModelAsync(componentModel),
                Breadcrumbs = await this.productBreadcrumbsModelBuilder.BuildModelAsync(componentModel),
                ProductDetail = await this.productDetailModelBuilder.BuildModelAsync(componentModel),
                Footer = await footerModelBuilder.BuildModelAsync(componentModel)
            };

            return viewModel;
        }
    }
}
