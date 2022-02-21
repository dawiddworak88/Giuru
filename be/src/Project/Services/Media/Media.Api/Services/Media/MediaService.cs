using Media.Api.Infrastructure;
using Media.Api.v1.Areas.Media.ResultModels;
using System;
using System.Threading.Tasks;
using System.Linq;
using Media.Api.ServicesModels;
using Media.Api.Repositories;
using Media.Api.Infrastructure.Media.Entities;
using MimeMapping;
using System.IO;
using Media.Api.Services.Checksums;
using Media.Api.Services.ImageResizers;
using System.Collections.Generic;
using Foundation.GenericRepository.Paginations;
using Foundation.GenericRepository.Predicates;
using Foundation.Extensions.ExtensionMethods;
using Media.Api.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.Extensions.Exceptions;
using System.Net;
using Microsoft.Extensions.Localization;
using Foundation.Localization;

namespace Media.Api.Services.Media
{
    public class MediaService : IMediaService
    {
        private readonly MediaContext context;
        private readonly IMediaRepository mediaRepository;
        private readonly IChecksumService checksumService;
        private readonly IImageResizeService imageResizeService;
        private readonly IStringLocalizer mediaResources;

        public MediaService(MediaContext context, 
            IMediaRepository mediaRepository, 
            IChecksumService checksumService,
            IImageResizeService imageResizeService,
            IStringLocalizer<MediaResources> mediaResources)
        {
            this.context = context;
            this.mediaRepository = mediaRepository;
            this.mediaResources = mediaResources;
            this.checksumService = checksumService;
            this.imageResizeService = imageResizeService;
        }

        public async Task<Guid> CreateFileAsync(CreateMediaItemServiceModel serviceModel)
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

            context.MediaItems.Add(mediaItem.FillCommonProperties());

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

            context.MediaItemVersions.Add(mediaItemVersion.FillCommonProperties());

            var mediaItemTranslation = new MediaItemTranslation
            {
                MediaItemVersionId = mediaItemVersion.Id,
                Language = serviceModel.Language,
                Name = Path.GetFileNameWithoutExtension(serviceModel.File.FileName)
            };

            context.MediaItemTranslations.Add(mediaItemTranslation.FillCommonProperties());
            context.SaveChanges();

            await this.mediaRepository.CreateFileAsync(mediaItemVersion.Id, serviceModel.OrganisationId.ToString(), serviceModel.File, serviceModel.File.FileName);

            return mediaItem.Id;
        }

        public async Task<Guid> UpdateFileAsync(UpdateMediaItemServiceModel serviceModel)
        {
            var existingMediaItemVersion = this.context.MediaItemVersions.Where(x => x.MediaItemId == serviceModel.Id.Value && x.IsActive).OrderBy(o => o.Version);
            if (existingMediaItemVersion is not null)
            {
                var checksum = this.checksumService.GetMd5(serviceModel.File);
                var version = existingMediaItemVersion.Count() + 1;
                var mediaItemVersion = new MediaItemVersion
                {
                    MediaItemId = serviceModel.Id.Value,
                    Filename = Path.GetFileNameWithoutExtension(serviceModel.File.FileName),
                    Extension = Path.GetExtension(serviceModel.File.FileName),
                    Folder = serviceModel.OrganisationId.ToString(),
                    MimeType = MimeUtility.GetMimeMapping(Path.GetExtension(serviceModel.File.FileName)),
                    Size = serviceModel.File.Length,
                    Checksum = checksum,
                    CreatedBy = serviceModel.Username,
                    Version = version
                };

                this.context.MediaItemVersions.Add(mediaItemVersion.FillCommonProperties());
                
                var translations = this.context.MediaItemTranslations.Where(x => x.MediaItemVersionId == existingMediaItemVersion.LastOrDefault().Id);
                foreach(var translation in translations)
                {
                    translation.MediaItemVersionId = mediaItemVersion.Id;
                    translation.LastModifiedDate = DateTime.UtcNow;
                }

                await this.context.SaveChangesAsync();
                await this.mediaRepository.CreateFileAsync(mediaItemVersion.Id, serviceModel.OrganisationId.ToString(), serviceModel.File, serviceModel.File.FileName);
            }

            return existingMediaItemVersion.FirstOrDefault().MediaItemId;
        }

        public async Task<MediaFileServiceModel> GetFileAsync(Guid? mediaId, bool? optimize, int? width, int? height)
        {
            if (mediaId.HasValue)
            {
                var mediaItem = (from m in this.context.MediaItems
                                 join mv in this.context.MediaItemVersions on m.Id equals mv.MediaItemId
                                 join t in this.context.MediaItemTranslations on mv.Id equals t.MediaItemVersionId into ct
                                 from x in ct.DefaultIfEmpty()
                                 where m.Id == mediaId.Value || mv.Id == mediaId.Value && m.IsActive == true && mv.IsActive && m.IsProtected == false
                                 orderby mv.Version descending
                                 select new MediaFileItemServiceModel
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

                        return new MediaFileServiceModel
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

        public PagedResults<IEnumerable<MediaItemServiceModel>> GetMediaItemsByIds(GetMediaItemsByIdsServiceModel model)
        {
            var unorderedMediaItemsResults = new List<MediaItemServiceModel>();
            var mediaItemsResults = new List<MediaItemServiceModel>();

            var predicateBuilder = PredicateBuilder.False<MediaItem>();

            foreach (var id in model.Ids.Skip((model.PageIndex - 1) * model.ItemsPerPage).Take(model.ItemsPerPage))
            {
                predicateBuilder = predicateBuilder.Or(x => x.Id == id);
            }

            var mediaItems = this.context.MediaItems.Where(x => x.IsActive).Where(predicateBuilder).ToList();

            foreach (var mediaItem in mediaItems.OrEmptyIfNull())
            {
                var mediaItemResult = this.MapMediaItemToMediaItemResultModel(mediaItem, model.Language);

                unorderedMediaItemsResults.Add(mediaItemResult);
            }

            foreach(var id in model.Ids.OrEmptyIfNull())
            {
                var mediaItemResult = unorderedMediaItemsResults.OrEmptyIfNull().FirstOrDefault(x => x.Id == id);

                if (mediaItemResult != null)
                {
                    mediaItemsResults.Add(mediaItemResult);
                }
            }

            return new PagedResults<IEnumerable<MediaItemServiceModel>>(unorderedMediaItemsResults.Count, model.ItemsPerPage)
            { 
                   Data = mediaItemsResults
            };
        }

        public MediaItemServiceModel GetMediaItemById(GetMediaItemsByIdServiceModel model)
        {
            var mediaItem = this.context.MediaItems.FirstOrDefault(x => x.Id == model.Id && x.IsActive);

            if (mediaItem != null)
            {
                return this.MapMediaItemToMediaItemResultModel(mediaItem, model.Language);
            }

            return default;
        }

        private MediaItemServiceModel MapMediaItemToMediaItemResultModel(MediaItem mediaItem, string language)
        {
            var mediaItemResult = new MediaItemServiceModel
            {
                Id = mediaItem.Id,
                IsProtected = mediaItem.IsProtected,
                LastModifiedDate = mediaItem.LastModifiedDate,
                CreatedDate = mediaItem.CreatedDate
            };

            var mediaItemVersion = this.context.MediaItemVersions.Where(x => x.MediaItemId == mediaItem.Id && x.IsActive).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

            if (mediaItemVersion != null)
            {
                mediaItemResult.Filename = $"{mediaItemVersion.Filename}{mediaItemVersion.Extension}";
                mediaItemResult.Size = mediaItemVersion.Size;
                mediaItemResult.MimeType = mediaItemVersion.MimeType;
                mediaItemResult.Extension = mediaItemVersion.Extension;

                var mediaItemVersionTranslation = this.context.MediaItemTranslations.FirstOrDefault(x => x.MediaItemVersionId == mediaItemVersion.Id && x.Language == language && x.IsActive);

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

            return mediaItemResult;
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

        public async Task<PagedResults<IEnumerable<MediaItemServiceModel>>> GetAsync(GetMediaItemsServiceModel serviceModel)
        {
            var mediaItems = from media in this.context.MediaItems
                             where media.IsActive == true && media.OrganisationId == serviceModel.OrganisationId.Value
                             select new MediaItemServiceModel
                             {
                                Id = media.Id,
                                Filename = media.Versions.FirstOrDefault().Filename,
                                Extension = media.Versions.FirstOrDefault().Extension,
                                MimeType = media.Versions.FirstOrDefault().MimeType,
                                Size = media.Versions.FirstOrDefault().Size,
                                Name = media.Versions.FirstOrDefault().Filename,
                                Description = media.Versions.FirstOrDefault().Translations.FirstOrDefault().Description,
                                LastModifiedDate = media.LastModifiedDate,
                                CreatedDate = media.CreatedDate,
                             };

            if (!string.IsNullOrWhiteSpace(serviceModel.SearchTerm))
            {
                mediaItems = mediaItems.Where(x => x.Filename.StartsWith(serviceModel.SearchTerm));
            }

            mediaItems = mediaItems.ApplySort(serviceModel.OrderBy);

            return mediaItems.PagedIndex(new Pagination(mediaItems.Count(), serviceModel.ItemsPerPage), serviceModel.PageIndex);
        }

        public async Task DeleteAsync(DeleteFileServiceModel model)
        {
            var mediaItem = this.context.MediaItems.FirstOrDefault(x => x.Id == model.MediaId.Value && x.IsActive);
            if (mediaItem == null)
            {
                throw new CustomException(this.mediaResources.GetString("MediaNotFound"), (int)HttpStatusCode.NotFound);
            }

            mediaItem.IsActive = false;
            mediaItem.LastModifiedDate = DateTime.UtcNow;

            await this.context.SaveChangesAsync();
        }

        public async Task<MediaItemVerionsByIdServiceModel> GetMediaItemVerionsByIdAsync(GetMediaItemsByIdServiceModel model)
        {
            var mediaItemVersions = this.context.MediaItemVersions
                .Where(x => x.MediaItemId == model.Id.Value)
                .Select(x => new MediaItemServiceModel
                {
                    Id = x.Id,
                    Filename = x.Filename,
                    Extension = x.Extension,
                    MimeType = x.MimeType,
                    Size = x.Size,
                    MediaItemId = x.MediaItemId,
                    Name = x.Translations.FirstOrDefault(x => x.Language == model.Language).Name,
                    Description = x.Translations.FirstOrDefault(x => x.Language == model.Language).Description,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate,
                }).OrderByDescending(x => x.CreatedDate).Take(5);

            if (mediaItemVersions.OrEmptyIfNull().Any())
            {
                var mediaItems = new MediaItemVerionsByIdServiceModel
                {
                    Id = model.Id.Value,
                    Name = mediaItemVersions.FirstOrDefault().Name,
                    Description = mediaItemVersions.FirstOrDefault().Description,
                    Versions = mediaItemVersions
                };
                
                return mediaItems;
            }

            return default;
        }

        public async Task UpdateMediaItemVersionAsync(UpdateMediaItemVersionServiceModel model)
        {
            var mediaVersion = this.context.MediaItemVersions.OrderBy(o => o.Version).LastOrDefault(x => x.MediaItemId == model.Id.Value && x.IsActive);
            if (mediaVersion is not null)
            {
                var mediaVersionTranslation = this.context.MediaItemTranslations.FirstOrDefault(x => x.MediaItemVersionId == mediaVersion.Id && x.Language == model.Language);
                if (mediaVersionTranslation is not null)
                {
                    mediaVersionTranslation.Name = model.Name;
                    mediaVersionTranslation.Description = model.Description;
                    mediaVersionTranslation.LastModifiedDate = DateTime.UtcNow;
                    mediaVersion.LastModifiedDate = DateTime.UtcNow;
                } else
                {
                    var mediaItemTranslation = new MediaItemTranslation
                    {
                        MediaItemVersionId = mediaVersion.Id,
                        Name = model.Name,
                        Description = model.Description,
                        Language = model.Language,
                    };

                    this.context.Add(mediaItemTranslation.FillCommonProperties());
                }

                await this.context.SaveChangesAsync();
            }
        }
    }
}
