using Foundation.GenericRepository.Paginations;
using Media.Api.v1.Areas.Media.Models;
using Media.Api.v1.Areas.Media.ResultModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Media.Api.v1.Areas.Media.Services
{
    public interface IMediaService
    {
        Task<MediaFileResultModel> GetFileAsync(Guid? mediaId, bool? optimize, int? width, int? height);
        Task<Guid> CreateFileAsync(CreateMediaItemModel serviceModel);
        PagedResults<IEnumerable<MediaItemResultModel>> GetMediaItemsByIds(GetMediaItemsByIdsModel model);
        MediaItemResultModel GetMediaItemById(GetMediaItemsByIdModel model);
    }
}
