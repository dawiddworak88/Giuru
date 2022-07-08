using DownloadCenter.Api.ServicesModels.DownloadCenter;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloadCenter.Api.Services.DownloadCenter
{
    public interface IDownloadCenterService
    {
        Task<PagedResults<IEnumerable<DownloadCenterServiceModel>>> GetAsync(GetDownloadCenterServiceModel model);
        Task<DownloadCategoriesServiceModel> GetAsync(GetDownloadCategoryServiceModel model);
    }
}
