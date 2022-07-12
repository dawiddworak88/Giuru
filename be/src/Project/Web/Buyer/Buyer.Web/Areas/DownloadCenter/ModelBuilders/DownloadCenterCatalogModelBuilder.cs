using Buyer.Web.Areas.DownloadCenter.Repositories;
using Buyer.Web.Areas.DownloadCenter.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.ModelBuilders
{
    public class DownloadCenterCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCatalogViewModel>
    {
        private readonly LinkGenerator linkGenerator;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IDownloadCenterRepository downloadCenterRepository;

        public DownloadCenterCatalogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IDownloadCenterRepository downloadCenterRepository,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.downloadCenterRepository = downloadCenterRepository;
        }

        public async Task<DownloadCenterCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadCenterCatalogViewModel
            {
                Title = "asd",
                TestUrl = this.linkGenerator.GetPathByAction("GetCategory", "DownloadCenterApi", new { Area = "Download", culture = CultureInfo.CurrentUICulture.Name }),
            };

            viewModel.PagedResults = await this.downloadCenterRepository.GetAsync(componentModel.Token, componentModel.Language, PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize, null, $"{nameof(DomainModels.DownloadCenterItem.Order)} desc");

            return viewModel;
        }
    }
}
