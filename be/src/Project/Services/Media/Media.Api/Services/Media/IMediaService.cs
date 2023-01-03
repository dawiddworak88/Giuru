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
        PagedResults<IEnumerable<MediaItemServiceModel>> Get(GetMediaItemsServiceModel serviceModel);
        Task<Guid> CreateFileAsync(CreateMediaItemServiceModel serviceModel);
        Task<Guid> UpdateFileAsync(UpdateMediaItemServiceModel serviceModel);
        MediaItemVerionsByIdServiceModel GetMediaItemVerionsById(GetMediaItemsByIdServiceModel model);
        Task UpdateMediaItemVersionAsync(UpdateMediaItemVersionServiceModel model);
        PagedResults<IEnumerable<MediaItemServiceModel>> GetMediaItemsByIds(GetMediaItemsByIdsServiceModel model);
        MediaItemServiceModel GetMediaItemById(GetMediaItemsByIdServiceModel model);
        Task DeleteAsync(DeleteFileServiceModel model);
        MediaFileServiceModel GetFile(Guid? mediaId);
        MediaFileServiceModel GetFileVersion(Guid? versionId);
        Task CreateFileChunkAsync(CreateFileChunkServiceModel model);
        Task<Guid> CreateFileFromChunksAsync(CreateMediaItemFromChunksServiceModel model);
        Task<Guid> UpdateFileFromChunksAsync(UpdateMediaItemFromChunksServiceModel model);
    }
}
