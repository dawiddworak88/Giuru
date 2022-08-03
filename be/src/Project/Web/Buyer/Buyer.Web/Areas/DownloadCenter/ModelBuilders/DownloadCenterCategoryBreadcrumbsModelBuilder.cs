using Buyer.Web.Areas.DownloadCenter.Repositories;
using Buyer.Web.Areas.DownloadCenter.ViewModel;
using Buyer.Web.Shared.ModelBuilders.Breadcrumbs;
using Buyer.Web.Shared.ViewModels.Breadcrumbs;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.ModelBuilders
{
    public class DownloadCenterCategoryBreadcrumbsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoryBreadcrumbsViewModel>
    {
        private readonly LinkGenerator linkGenerator;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IDownloadCenterRepository downloadCenterRepository;
        private readonly IBreadcrumbsModelBuilder<ComponentModelBase, DownloadCenterCategoryBreadcrumbsViewModel> breadcrumbsModelBuilder;

        public DownloadCenterCategoryBreadcrumbsModelBuilder(
            LinkGenerator linkGenerator,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IDownloadCenterRepository downloadCenterRepository,
            IBreadcrumbsModelBuilder<ComponentModelBase, DownloadCenterCategoryBreadcrumbsViewModel> breadcrumbsModelBuilder)
        {
            this.linkGenerator = linkGenerator;
            this.breadcrumbsModelBuilder = breadcrumbsModelBuilder;
            this.downloadCenterRepository = downloadCenterRepository;
            this.globalLocalizer = globalLocalizer;
        }

        public async Task<DownloadCenterCategoryBreadcrumbsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.breadcrumbsModelBuilder.BuildModel(componentModel);

            viewModel.Items.Add(new BreadcrumbViewModel
            {
                Name = this.globalLocalizer.GetString("DownloadCenter"),
                Url = this.linkGenerator.GetPathByAction("Index", "DownloadCenter", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name }),
                IsActive = false
            });

            if (componentModel.Id.HasValue)
            {
                var downloadCenterCategory = await this.downloadCenterRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (downloadCenterCategory is not null)
                {
                    if (downloadCenterCategory.ParentCategoryId.HasValue)
                    {
                        viewModel.Items.Add(new BreadcrumbViewModel
                        {
                            Name = downloadCenterCategory.ParentCategoryName,
                            Url = this.linkGenerator.GetPathByAction("Detail", "Category", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name, Id = downloadCenterCategory.ParentCategoryId }),
                            IsActive = false
                        });
                    }

                    viewModel.Items.Add(new BreadcrumbViewModel
                    {
                        Name = downloadCenterCategory.CategoryName,
                        Url = this.linkGenerator.GetPathByAction("Detail", "Category", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name, Id = downloadCenterCategory.Id }),
                        IsActive = true
                    });
                }
            }

            return viewModel;
        }
    }
}
