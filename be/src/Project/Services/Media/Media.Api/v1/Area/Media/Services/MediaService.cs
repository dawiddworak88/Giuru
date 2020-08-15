using Media.Api.Infrastructure;
using Media.Api.v1.Area.Media.ResultModels;
using System;
using System.Threading.Tasks;
using System.Linq;
using Media.Api.v1.Area.Media.Models;
using Media.Api.v1.Area.Media.Repositories;

namespace Media.Api.v1.Area.Media.Services
{
    public class MediaService : IMediaService
    {
        private readonly MediaContext context;
        private readonly IMediaRepository mediaRepository;

        public MediaService(MediaContext context, IMediaRepository mediaRepository)
        {
            this.context = context;
            this.mediaRepository = mediaRepository;
        }

        public async Task<MediaItemResultModel> GetMediaItemAsync(Guid? mediaId)
        {
            if (mediaId.HasValue)
            {
                var mediaItem = (from m in this.context.MediaItems
                                 join mv in this.context.MediaItemVersions on m.Id equals mv.MediaItemId
                                 join t in this.context.MediaItemTranslations on mv.Id equals t.MediaItemVersionId into ct
                                 from x in ct.DefaultIfEmpty()
                                 where m.Id == mediaId && m.IsActive == true && mv.IsActive && m.IsProtected == false
                                 orderby mv.Version descending
                                 select new MediaItemModel
                                 {
                                     Id = m.Id,
                                     VersionId = mv.Id,
                                     ContentType = mv.MimeType,
                                     Filename = mv.Filename,
                                     Extension = mv.Extension,
                                     Folder = mv.Folder,
                                 }).FirstOrDefault();

                if (mediaItem != null)
                {
                    var file = await this.mediaRepository.GetFileAsync(mediaItem.Folder, $"{mediaItem.VersionId}{mediaItem.Extension}");

                    if (file != null)
                    {
                        return new MediaItemResultModel
                        {
                            Id = mediaItem.Id,
                            Filename = $"{mediaItem.Filename}{mediaItem.Extension}",
                            ContentType = mediaItem.ContentType,
                            File = file
                        };
                    }
                }
            }

            return default;
        }
    }
}
