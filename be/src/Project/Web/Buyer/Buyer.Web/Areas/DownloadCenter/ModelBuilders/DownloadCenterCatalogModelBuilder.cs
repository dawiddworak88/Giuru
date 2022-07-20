using Buyer.Web.Areas.DownloadCenter.Repositories;
using Buyer.Web.Areas.DownloadCenter.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.ModelBuilders
{
    public class DownloadCenterCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCatalogViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IDownloadCenterRepository downloadCenterRepository;

        public DownloadCenterCatalogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IDownloadCenterRepository downloadCenterRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.downloadCenterRepository = downloadCenterRepository;
        }

        public async Task<DownloadCenterCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadCenterCatalogViewModel
            {
                NoCategoriesLabel = this.globalLocalizer.GetString("NoCategories")
            };

            viewModel.PagedResults = await this.downloadCenterRepository.GetAsync(componentModel.Token, componentModel.Language, PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize, null, $"{nameof(DomainModels.DownloadCenterItem.CreatedDate)} asc");

            return viewModel;
        }
    }
}
