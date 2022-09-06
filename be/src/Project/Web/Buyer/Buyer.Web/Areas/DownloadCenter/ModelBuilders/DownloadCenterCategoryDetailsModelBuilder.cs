using Buyer.Web.Areas.DownloadCenter.DomainModels;
using Buyer.Web.Areas.DownloadCenter.Repositories;
using Buyer.Web.Areas.DownloadCenter.ViewModel;
using Buyer.Web.Shared.ComponentModels.Files;
using Buyer.Web.Shared.ViewModels.Files;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.ModelBuilders
{
    public class DownloadCenterCategoryDetailsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoryDetailsViewModel>
    {
        private readonly IAsyncComponentModelBuilder<DownloadCenterFilesComponentModel, DownloadCenterFilesViewModel> downloadCenterFilesModelBuilder;
        private readonly IDownloadCenterRepository downloadCenterRepository;
        private readonly LinkGenerator linkGenerator;

        public DownloadCenterCategoryDetailsModelBuilder(
            IAsyncComponentModelBuilder<DownloadCenterFilesComponentModel, DownloadCenterFilesViewModel> downloadCenterFilesModelBuilder,
            IDownloadCenterRepository downloadCenterRepository,
            LinkGenerator linkGenerator)
        {
            this.downloadCenterFilesModelBuilder = downloadCenterFilesModelBuilder;
            this.downloadCenterRepository = downloadCenterRepository;
            this.linkGenerator = linkGenerator;
        }

        public async Task<DownloadCenterCategoryDetailsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadCenterCategoryDetailsViewModel();

            if (componentModel.Id.HasValue)
            {
                var downloadCenterCategory = await this.downloadCenterRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (downloadCenterCategory is not null)
                {
                    viewModel.Title = downloadCenterCategory.CategoryName;
                    viewModel.Subcategories = downloadCenterCategory.Subcategories.OrEmptyIfNull().Select(x => new DownloadCenterCategoryDetailViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Url = x.Url
                    });
                }

                var downloadCenterFiles = await this.downloadCenterRepository.GetCategoryFilesAsync(componentModel.Token, componentModel.Language, componentModel.Id, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, null, $"{nameof(DownloadCenterFile.CreatedDate)} desc");

                if (downloadCenterFiles is not null)
                {
                    var filesComponentModel = new DownloadCenterFilesComponentModel
                    {
                        Id = componentModel.Id,
                        Token = componentModel.Token,
                        Language = componentModel.Language,
                        Files = downloadCenterFiles.Data.Select(x => x.Id),
                        SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "DownloadCenterApi", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name, Id = componentModel.Id })
                    };

                    viewModel.Files = await this.downloadCenterFilesModelBuilder.BuildModelAsync(filesComponentModel);
                }
            }

            return viewModel;
        }
    }
}
