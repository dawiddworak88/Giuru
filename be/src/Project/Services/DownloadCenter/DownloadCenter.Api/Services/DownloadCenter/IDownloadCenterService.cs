using DownloadCenter.Api.ServicesModels.DownloadCenter;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloadCenter.Api.Services.DownloadCenter
{
    public interface IDownloadCenterService
    {
        Task<PagedResults<IEnumerable<DownloadCenterServiceModel>>> GetAsync(GetDownloadCenterServiceModel model);
        Task<DownloadCenterCategoriesServiceModel> GetDownloadCenterCategoryAsync(GetDownloadCenterCategoryServiceModel model);
        Task<DownloadCenterItemServiceModel> GetAsync(GetDownloadCenterItemServiceModel model);
        Task DeleteAsync(DeleteDownloadCenterItemServiceModel model);
        Task<Guid> CreateAsync(CreateDownloadCenterItemServiceModel model);
        Task<Guid> UpdateAsync(UpdateDownloadCenterItemServiceModel model);
    }
}
