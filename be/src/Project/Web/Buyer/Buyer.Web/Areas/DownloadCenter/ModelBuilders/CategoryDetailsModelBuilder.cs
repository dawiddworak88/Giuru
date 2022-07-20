using Buyer.Web.Areas.DownloadCenter.Repositories;
using Buyer.Web.Areas.DownloadCenter.ViewModel;
using Buyer.Web.Shared.ComponentModels.Files;
using Buyer.Web.Shared.ViewModels.Files;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.ModelBuilders
{
    public class CategoryDetailsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoryDetailsViewModel>
    {
        private readonly IAsyncComponentModelBuilder<FilesComponentModel, DownloadCenterFilesViewModel> downloadCenterFilesModelBuilder;
        private readonly IDownloadCenterRepository downloadCenterRepository;

        public CategoryDetailsModelBuilder(
            IAsyncComponentModelBuilder<FilesComponentModel, DownloadCenterFilesViewModel> downloadCenterFilesModelBuilder,
            IDownloadCenterRepository downloadCenterRepository)
        {
            this.downloadCenterFilesModelBuilder = downloadCenterFilesModelBuilder;
            this.downloadCenterRepository = downloadCenterRepository;
        }

        public async Task<CategoryDetailsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new CategoryDetailsViewModel();

            if (componentModel.Id.HasValue)
            {
                var downloadCenterCategory = await this.downloadCenterRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (downloadCenterCategory is not null)
                {
                    viewModel.Title = downloadCenterCategory.CategoryName;
                    viewModel.Subcategories = downloadCenterCategory.Subcategories.OrEmptyIfNull().Select(x => new CategoryDetailViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Url = x.Url
                    });

                    if (downloadCenterCategory.Files.OrEmptyIfNull().Any())
                    {
                        viewModel.Files = await this.downloadCenterFilesModelBuilder.BuildModelAsync(new FilesComponentModel { Language = componentModel.Language, Token = componentModel.Token, Files = downloadCenterCategory.Files });
                    }
                }
            }

            return viewModel;
        }
    }
}
