using Media.Api.v1.Area.Media.Models;
using Media.Api.v1.Area.Media.ResultModels;
using System;
using System.Threading.Tasks;

namespace Media.Api.v1.Area.Media.Services
{
    public interface IMediaService
    {
        Task<MediaFileResultModel> GetMediaItemAsync(Guid? mediaId, int? width, int? height);
        Task<Guid> CreateMediaItemAsync(CreateMediaItemModel serviceModel);
    }
}
