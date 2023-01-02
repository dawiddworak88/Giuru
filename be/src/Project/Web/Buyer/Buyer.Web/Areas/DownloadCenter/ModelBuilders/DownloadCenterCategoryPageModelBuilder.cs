using Buyer.Web.Areas.DownloadCenter.ViewModel;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.ModelBuilders
{
    public class DownloadCenterCategoryPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoryPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoryDetailsViewModel> categoryDetailsModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoryBreadcrumbsViewModel> categoryBreadcrumbsModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public DownloadCenterCategoryPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoryDetailsViewModel> categoryDetailsModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoryBreadcrumbsViewModel> categoryBreadcrumbsModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.seoModelBuilder = seoModelBuilder;
            this.headerModelBuilder = headerModelBuilder;
            this.mainNavigationModelBuilder = mainNavigationModelBuilder;
            this.categoryDetailsModelBuilder = categoryDetailsModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.categoryBreadcrumbsModelBuilder = categoryBreadcrumbsModelBuilder;
        }

        public async Task<DownloadCenterCategoryPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadCenterCategoryPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Metadata = await this.seoModelBuilder.BuildModelAsync(componentModel),
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await this.mainNavigationModelBuilder.BuildModelAsync(componentModel),
                Breadcrumbs = await this.categoryBreadcrumbsModelBuilder.BuildModelAsync(componentModel),
                CategoryDetails = await this.categoryDetailsModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
