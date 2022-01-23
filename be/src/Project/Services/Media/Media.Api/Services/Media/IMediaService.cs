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
        Task<MediaFileServiceModel> GetFileAsync(Guid? mediaId, int? width, int? height, bool optimize, string? extension);
        Task<Guid> CreateFileAsync(CreateMediaItemServiceModel serviceModel);
        PagedResults<IEnumerable<MediaItemServiceModel>> GetMediaItemsByIds(GetMediaItemsByIdsServiceModel model);
        MediaItemServiceModel GetMediaItemById(GetMediaItemsByIdServiceModel model);
    }
}
