using Media.Api.v1.Area.Media.ResultModels;
using System;
using System.Threading.Tasks;

namespace Media.Api.v1.Area.Media.Services
{
    public interface IMediaService
    {
        Task<MediaItemResultModel> GetMediaItemAsync(Guid? mediaId);
    }
}
