using Buyer.Web.Areas.Download.Repositories;
using Buyer.Web.Areas.Download.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Download.ModelBuilders
{
    public class DownloadCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadCatalogViewModel>
    {
        private readonly LinkGenerator linkGenerator;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IDownloadsRepository downloadsRepository;

        public DownloadCatalogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IDownloadsRepository downloadsRepository,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.downloadsRepository = downloadsRepository;
        }

        public async Task<DownloadCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadCatalogViewModel
            {
                Title = "asd",
                TestUrl = this.linkGenerator.GetPathByAction("GetCategory", "DownloadsApi", new { Area = "Download", culture = CultureInfo.CurrentUICulture.Name }),
            };

            viewModel.PagedResults = await this.downloadsRepository.GetAsync(componentModel.Token, componentModel.Language, PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize, null, $"{nameof(DomainModels.Download.Order)} desc");

            return viewModel;
        }
    }
}
