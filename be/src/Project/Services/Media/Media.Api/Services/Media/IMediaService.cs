using Foundation.GenericRepository.Paginations;
using Media.Api.ServicesModels;
using Media.Api.v1.Areas.Media.ResultModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Media.Api.Services.Media
{
    public interface IMediaService
    {
        Task<PagedResults<IEnumerable<MediaItemServiceModel>>> GetAsync(GetMediaItemsServiceModel serviceModel);
        MediaFileServiceModel GetFile(Guid? mediaId, int? width, int? height, bool optimize, string? extension);
        Task<Guid> CreateFileAsync(CreateMediaItemServiceModel serviceModel);
        Task<Guid> UpdateFileAsync(UpdateMediaItemServiceModel serviceModel);
        Task<MediaItemVerionsByIdServiceModel> GetMediaItemVerionsByIdAsync(GetMediaItemsByIdServiceModel model);
        Task UpdateMediaItemVersionAsync(UpdateMediaItemVersionServiceModel model);
        PagedResults<IEnumerable<MediaItemServiceModel>> GetMediaItemsByIds(GetMediaItemsByIdsServiceModel model);
        MediaItemServiceModel GetMediaItemById(GetMediaItemsByIdServiceModel model);
        Task DeleteAsync(DeleteFileServiceModel model);
        Task CreateFileChunkAsync(CreateFileChunkServiceModel model);
        Task<Guid> CreateFileFromChunksAsync(CreateMediaItemFromChunksServiceModel model);
        Task<Guid> UpdateFileFromChunksAsync(UpdateMediaItemFromChunksServiceModel model);
    }
}
