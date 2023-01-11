using DownloadCenter.Api.ServicesModels.DownloadCenter;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloadCenter.Api.Services.DownloadCenter
{
    public interface IDownloadCenterService
    {
        Task<PagedResults<IEnumerable<DownloadCenterItemServiceModel>>> GetAsync(GetDownloadCenterFilesServiceModel model);
        DownloadCenterCategoryServiceModel GetDownloadCenterCategory(GetDownloadCenterCategoryServiceModel model);
        PagedResults<IEnumerable<DownloadCenterCategoryItemServiceModel>> Get(GetDownloadCenterItemsServiceModel model);
        DownloadCenterItemFileServiceModel Get(GetDownloadCenterFileServiceModel model);
        Task DeleteAsync(DeleteDownloadCenterItemServiceModel model);
        Task<Guid> CreateAsync(CreateDownloadCenterItemServiceModel model);
        Task<Guid> UpdateAsync(UpdateDownloadCenterItemServiceModel model);
        Task UpdateFileNameAsync(Guid? id, string name);
        PagedResults<IEnumerable<DownloadCenterCategoryFileServiceModel>> GetDownloadCenterCategoryFiles(GetDownloadCenterCategoryFilesServiceModel model);
    }
}
