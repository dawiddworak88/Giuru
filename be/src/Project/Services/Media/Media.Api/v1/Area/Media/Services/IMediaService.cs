using Foundation.GenericRepository.Paginations;
using Media.Api.v1.Area.Media.Models;
using Media.Api.v1.Area.Media.ResultModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Media.Api.v1.Area.Media.Services
{
    public interface IMediaService
    {
        Task<MediaFileResultModel> GetFileAsync(Guid? mediaId, bool? optimize, int? width, int? height);
        Task<Guid> CreateFileAsync(CreateMediaItemModel serviceModel);
        PagedResults<IEnumerable<MediaItemResultModel>> GetMediaItemsByIds(GetMediaItemsByIdsModel model);
    }
}
