using DownloadCenter.Api.ServicesModels.DownloadCenter;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloadCenter.Api.Services.DownloadCenter
{
    public interface IDownloadCenterService
    {
        Task<PagedResults<IEnumerable<DownloadCenterFileServiceModel>>> GetAsync(GetDownloadCenterFilesServiceModel model);
        Task<DownloadCenterCategoryServiceModel> GetDownloadCenterCategoryAsync(GetDownloadCenterFilesCategoryServiceModel model);
        Task<PagedResults<IEnumerable<DownloadCenterItemServiceModel>>> GetAsync(GetDownloadCenterItemsServiceModel model);
        Task<DownloadCenterCategoriesServiceModel> GetAsync(GetDownloadCenterFileServiceModel model);
        Task DeleteAsync(DeleteDownloadCenterFileServiceModel model);
        Task<Guid> CreateAsync(CreateDownloadCenterFileServiceModel model);
        Task<Guid> UpdateAsync(UpdateDownloadCenterFileServiceModel model);
        Task UpdateFileNameAsync(Guid? id, string name);
    }
}
