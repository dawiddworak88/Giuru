using DownloadCenter.Api.ServicesModels.DownloadCenter;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloadCenter.Api.Services.DownloadCenter
{
    public interface IDownloadCenterService
    {
        Task<PagedResults<IEnumerable<DownloadCenterServiceModel>>> GetAsync(GetDownloadCenterFilesServiceModel model);
        Task<DownloadCenterCategoryFilesServiceModel> GetDownloadCenterCategoryAsync(GetDownloadCenterCategoryFilesServiceModel model);
        Task<PagedResults<IEnumerable<DownloadCenterItemServiceModel>>> GetTestAsync(GetDownloadCenterItemsServiceModel model);
        Task<DownloadCenterFileServiceModel> GetAsync(GetDownloadCenterFileServiceModel model);
        Task DeleteAsync(DeleteDownloadCenterFileServiceModel model);
        Task<Guid> CreateAsync(CreateDownloadCenterFileServiceModel model);
        Task<Guid> UpdateAsync(UpdateDownloadCenterFileServiceModel model);
    }
}
