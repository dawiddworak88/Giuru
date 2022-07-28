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
using Microsoft.EntityFrameworkCore;
using Media.Api.IntegrationEvents;
using Foundation.EventBus.Abstractions;

namespace Media.Api.Services.Media
{
    public class MediaService : IMediaService
    {
        private readonly MediaContext context;
        private readonly IMediaRepository mediaRepository;
        private readonly IChecksumService checksumService;
        private readonly IStringLocalizer mediaResources;
        private readonly IEventBus eventBus;

        public MediaService(MediaContext context, 
            IMediaRepository mediaRepository, 
            IChecksumService checksumService,
            IEventBus eventBus,
            IStringLocalizer<MediaResources> mediaResources)
        {
            this.context = context;
            this.mediaRepository = mediaRepository;
            this.mediaResources = mediaResources;
            this.checksumService = checksumService;
            this.eventBus = eventBus;
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

        public MediaFileServiceModel GetFile(Guid? mediaId, int? width, int? height, bool optimize, string? extension)
        {
            if (mediaId.HasValue)
            {
                var mediaItem = (from m in this.context.MediaItems
                                 join mv in this.context.MediaItemVersions on m.Id equals mv.MediaItemId
                                 where m.Id == mediaId.Value && m.IsActive == true && mv.IsActive && m.IsProtected == false
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
                    var file = this.mediaRepository.GetFile(mediaItem.Folder, $"{mediaItem.VersionId}{mediaItem.Extension}");

                    if (file != null)
                    {
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

            return new PagedResults<IEnumerable<MediaItemServiceModel>>(model.Ids.Count(), model.ItemsPerPage)
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
                MediaConstants.MimeTypes.Svg,
                MediaConstants.MimeTypes.Webp
            };

            return imageContentTypes.Contains(contentType);
        }

        public async Task<PagedResults<IEnumerable<MediaItemServiceModel>>> GetAsync(GetMediaItemsServiceModel serviceModel)
        {
            var mediaItems = this.context.MediaItems.Where(x => x.IsActive && x.OrganisationId == serviceModel.OrganisationId.Value);

            if (string.IsNullOrWhiteSpace(serviceModel.SearchTerm) is false)
            {
                mediaItems = mediaItems.Where(x => x.Versions.Any(x => x.Filename.StartsWith(serviceModel.SearchTerm)));
            }

            mediaItems = mediaItems.ApplySort(serviceModel.OrderBy);

            var pagedResults = mediaItems.PagedIndex(new Pagination(mediaItems.Count(), serviceModel.ItemsPerPage), serviceModel.PageIndex);

            var pagedMediaItemsServiceModel = new PagedResults<IEnumerable<MediaItemServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var items = new List<MediaItemServiceModel>();

            foreach (var mediaItem in pagedResults.Data.ToList())
            {
                var item = new MediaItemServiceModel
                {
                    Id = mediaItem.Id,
                    LastModifiedDate = mediaItem.LastModifiedDate,
                    CreatedDate = mediaItem.CreatedDate
                };

                var mediaItemVersion = await this.context.MediaItemVersions.OrderBy(x => x.Version).LastOrDefaultAsync(x => x.MediaItemId == mediaItem.Id);

                if (mediaItemVersion is not null)
                {
                    item.MediaItemVersionId = mediaItemVersion.Id;
                    item.Filename = mediaItemVersion.Filename;
                    item.Extension = mediaItemVersion.Extension;
                    item.MimeType = mediaItemVersion.MimeType;
                    item.Size = mediaItemVersion.Size;

                    var mediaItemVersionTranslation = await this.context.MediaItemTranslations.FirstOrDefaultAsync(x => x.MediaItemVersionId == mediaItemVersion.Id && x.Language == serviceModel.Language && x.IsActive);

                    if (mediaItemVersionTranslation is null)
                    {
                        mediaItemVersionTranslation = await this.context.MediaItemTranslations.FirstOrDefaultAsync(x => x.MediaItemVersionId == mediaItemVersion.Id && x.IsActive);
                    }

                    item.Name = mediaItemVersionTranslation?.Name;
                    item.Description = mediaItemVersionTranslation?.Description;
                    item.MetaData = mediaItemVersionTranslation?.Metadata;
                }

                items.Add(item);
            }

            pagedMediaItemsServiceModel.Data = items;

            return pagedMediaItemsServiceModel;
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
                    MetaData = x.Translations.FirstOrDefault(x => x.Language == model.Language).Metadata,
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
                    MetaData = mediaItemVersions.FirstOrDefault().MetaData,
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
                    mediaVersionTranslation.Metadata = model.MetaData;
                    mediaVersionTranslation.LastModifiedDate = DateTime.UtcNow;
                    mediaVersion.LastModifiedDate = DateTime.UtcNow;
                } else
                {
                    var mediaItemTranslation = new MediaItemTranslation
                    {
                        MediaItemVersionId = mediaVersion.Id,
                        Name = model.Name,
                        Description = model.Description,
                        Metadata = model.MetaData,
                        Language = model.Language,
                    };

                    this.context.Add(mediaItemTranslation.FillCommonProperties());
                }

                await this.context.SaveChangesAsync();

                var message = new UpdatedFileIntegrationEvent
                {
                    FileId = model.Id.Value,
                    Name = model.Name
                };

                this.eventBus.Publish(message);
            }
        }
    }
}
