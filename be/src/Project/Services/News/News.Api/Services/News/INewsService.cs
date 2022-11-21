using Foundation.GenericRepository.Paginations;
using News.Api.ServicesModels.News;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace News.Api.Services.News
{
    public interface INewsService
    {
        Task<PagedResults<IEnumerable<NewsItemServiceModel>>> GetAsync(GetNewsItemsServiceModel model);
        Task<NewsItemServiceModel> GetAsync(GetNewsItemServiceModel model);
        Task DeleteAsync(DeleteNewsItemServiceModel model);
        Task<Guid> CreateAsync(CreateNewsItemServiceModel model);
        Task<Guid> UpdateAsync(UpdateNewsItemServiceModel model);
        Task<PagedResults<IEnumerable<NewsItemFileServiceModel>>> GetFilesAsync(GetNewsItemFilesServiceModel model);
    }
}
