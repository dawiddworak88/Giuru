using Media.Api.Infrastructure;
using Media.Api.v1.Area.Media.ResultModels;
using System;
using System.Threading.Tasks;
using System.Linq;
using Media.Api.v1.Area.Media.Models;
using Media.Api.v1.Area.Media.Repositories;
using Media.Api.Infrastructure.Media.Entities;
using Foundation.GenericRepository.Helpers;
using MimeMapping;
using System.IO;
using Media.Api.Shared.Checksums;

namespace Media.Api.v1.Area.Media.Services
{
    public class MediaService : IMediaService
    {
        private readonly MediaContext context;
        private readonly IMediaRepository mediaRepository;
        private readonly IChecksumService checksumService;

        public MediaService(MediaContext context, IMediaRepository mediaRepository, IChecksumService checksumService)
        {
            this.context = context;
            this.mediaRepository = mediaRepository;
            this.checksumService = checksumService;
        }

        public async Task<Guid> CreateMediaItemAsync(CreateMediaItemModel serviceModel)
        {
            var checksum = this.checksumService.GetMd5(serviceModel.File.OpenReadStream());

            var existingMediaItemVersion = context.MediaItemVersions.FirstOrDefault(x => x.Checksum == checksum && x.Filename == Path.GetFileNameWithoutExtension(serviceModel.File.FileName) && x.IsActive);

            if (existingMediaItemVersion != null)
            {
                return existingMediaItemVersion.MediaItemId;
            }

            var mediaItem = new MediaItem
            {
                OrganisationId = serviceModel.OrganisationId,
                IsProtected = false
            };

            context.MediaItems.Add(EntitySeedHelper.SeedEntity(mediaItem));

            var mediaItemVersion = new MediaItemVersion
            {
                MediaItemId = mediaItem.Id,
                Filename = Path.GetFileNameWithoutExtension(serviceModel.File.FileName),
                Extension = Path.GetExtension(serviceModel.File.FileName),
                Folder = serviceModel.OrganisationId.ToString(),
                MimeType = MimeUtility.GetMimeMapping(Path.GetExtension(serviceModel.File.FileName)),
                Size = serviceModel.File.Length,
                Checksum = checksum,
                CreatedBy = serviceModel.Username,
                Version = 1
            };

            context.MediaItemVersions.Add(EntitySeedHelper.SeedEntity(mediaItemVersion));

            var mediaItemTranslation = new MediaItemTranslation
            {
                MediaItemVersionId = mediaItemVersion.Id,
                Language = serviceModel.Language,
                Name = Path.GetFileNameWithoutExtension(serviceModel.File.FileName)
            };

            context.MediaItemTranslations.Add(EntitySeedHelper.SeedEntity(mediaItemTranslation));

            context.SaveChanges();

            await this.mediaRepository.CreateFileAsync(mediaItemVersion.Id, serviceModel.OrganisationId.ToString(), serviceModel.File.OpenReadStream(), serviceModel.File.FileName);

            return mediaItem.Id;
        }

        public async Task<MediaFileResultModel> GetMediaItemAsync(Guid? mediaId)
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
                        return new MediaFileResultModel
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
