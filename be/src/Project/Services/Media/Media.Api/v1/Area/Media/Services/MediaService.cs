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
using Media.Api.Shared.ImageResizers;
using System.Collections.Generic;
using Foundation.GenericRepository.Paginations;
using Foundation.GenericRepository.Predicates;
using Foundation.Extensions.ExtensionMethods;
using Media.Api.Definitions;

namespace Media.Api.v1.Area.Media.Services
{
    public class MediaService : IMediaService
    {
        private readonly MediaContext context;
        private readonly IMediaRepository mediaRepository;
        private readonly IChecksumService checksumService;
        private readonly IImageResizeService imageResizeService;

        public MediaService(MediaContext context, 
            IMediaRepository mediaRepository, 
            IChecksumService checksumService,
            IImageResizeService imageResizeService)
        {
            this.context = context;
            this.mediaRepository = mediaRepository;
            this.checksumService = checksumService;
            this.imageResizeService = imageResizeService;
        }

        public async Task<Guid> CreateFileAsync(CreateMediaItemModel serviceModel)
        {
            var checksum = this.checksumService.GetMd5(serviceModel.File);

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

            await this.mediaRepository.CreateFileAsync(mediaItemVersion.Id, serviceModel.OrganisationId.ToString(), serviceModel.File, serviceModel.File.FileName);

            return mediaItem.Id;
        }

        public async Task<MediaFileResultModel> GetFileAsync(Guid? mediaId, bool? optimize, int? width, int? height)
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
                        if (this.IsImage(mediaItem.ContentType))
                        {
                            if (width.HasValue && height.HasValue)
                            {
                                file = this.imageResizeService.Resize(file, width.Value, height.Value, optimize.HasValue && optimize.Value, mediaItem.ContentType);
                            }

                            if (optimize.HasValue && optimize.Value)
                            {
                                file = this.imageResizeService.Optimize(file, mediaItem.ContentType);
                            }
                        }

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

        public PagedResults<IEnumerable<MediaItemResultModel>> GetMediaItemsByIds(GetMediaItemsByIdsModel model)
        {
            var mediaItemResult = new MediaItemResultModel();

            var mediaItemsResults = new List<MediaItemResultModel>();

            var predicateBuilder = PredicateBuilder.False<MediaItem>();

            foreach (var id in model.Ids)
            {
                predicateBuilder = predicateBuilder.Or(x => x.Id == id);
            }

            var mediaItems = this.context.MediaItems.Where(x => x.IsActive).Where(predicateBuilder).ToList();

            foreach (var mediaItem in mediaItems.OrEmptyIfNull())
            {
                mediaItemResult.Id = mediaItem.Id;
                mediaItemResult.IsProtected = mediaItem.IsProtected;
                mediaItemResult.LastModifiedDate = mediaItem.LastModifiedDate;
                mediaItemResult.CreatedDate = mediaItem.CreatedDate;

                var mediaItemVersion = this.context.MediaItemVersions.Where(x => x.MediaItemId == mediaItem.Id && x.IsActive).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (mediaItemVersion != null)
                {
                    mediaItemResult.Filename = $"{mediaItemVersion.Filename}{mediaItemVersion.Extension}";
                    mediaItemResult.Size = mediaItemVersion.Size;

                    var mediaItemVersionTranslation = this.context.MediaItemTranslations.FirstOrDefault(x => x.MediaItemVersionId == mediaItemVersion.Id && x.Language == model.Language && x.IsActive);

                    if (mediaItemVersionTranslation == null)
                    {
                        mediaItemVersionTranslation = this.context.MediaItemTranslations.FirstOrDefault(x => x.MediaItemVersionId == mediaItemVersion.Id && x.IsActive);
                    }

                    if (mediaItemVersionTranslation != null)
                    {
                        mediaItemResult.Name = mediaItemVersionTranslation.Name;
                        mediaItemResult.Description = mediaItemVersionTranslation.Description;
                    }
                }

                mediaItemsResults.Add(mediaItemResult);
            }

            return new PagedResults<IEnumerable<MediaItemResultModel>>(mediaItemsResults.Count, PaginationConstants.DefaultPageIndex)
            { 
                Data = mediaItemsResults,
                
            };
        }

        private bool IsImage(string contentType)
        {
            var imageContentTypes = new List<string>
            {
                MediaConstants.MimeTypes.Jpeg,
                MediaConstants.MimeTypes.Png,
                MediaConstants.MimeTypes.Svg
            };

            return imageContentTypes.Contains(contentType);
        }
    }
}
