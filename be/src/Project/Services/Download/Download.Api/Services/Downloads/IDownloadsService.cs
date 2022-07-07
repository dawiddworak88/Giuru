using Download.Api.ServicesModels.Downloads;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Download.Api.Services.Downloads
{
    public interface IDownloadsService
    {
        Task<PagedResults<IEnumerable<DownloadServiceModel>>> GetAsync(GetDownloadsServiceModel model);
        Task<DownloadCategoriesServiceModel> GetAsync(GetDownloadCategoryServiceModel model);
    }
}
