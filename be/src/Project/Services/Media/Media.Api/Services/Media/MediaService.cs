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
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Media.Api.IntegrationEvents;
using Foundation.EventBus.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Foundation.GenericRepository.Definitions;
using System.Net;

namespace Media.Api.Services.Media
{
    public class MediaService : IMediaService
    {
        private readonly MediaContext _context;
        private readonly IMediaRepository _mediaRepository;
        private readonly IChecksumService _checksumService;
        private readonly IStringLocalizer _mediaResources;
        private readonly IEventBus _eventBus;

        public MediaService(MediaContext context, 
            IMediaRepository mediaRepository, 
            IChecksumService checksumService,
            IEventBus eventBus,
            IStringLocalizer<MediaResources> mediaResources)
        {
            _context = context;
            _mediaRepository = mediaRepository;
            _mediaResources = mediaResources;
            _checksumService = checksumService;
            _eventBus = eventBus;
        }

        public async Task<Guid> CreateFileAsync(CreateMediaItemServiceModel serviceModel)
        {
            var checksum = _checksumService.GetMd5(serviceModel.File);

            var existingMediaItemVersion = _context.MediaItemVersions.FirstOrDefault(x => x.Checksum == checksum && x.Filename == Path.GetFileNameWithoutExtension(serviceModel.File.FileName) && x.IsActive);

            if (existingMediaItemVersion is not null)
            {
                return existingMediaItemVersion.MediaItemId;
            }

            var mediaItem = new MediaItem
            {
                OrganisationId = serviceModel.OrganisationId,
                IsProtected = false
            };

            _context.MediaItems.Add(mediaItem.FillCommonProperties());

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

            _context.MediaItemVersions.Add(mediaItemVersion.FillCommonProperties());

            var mediaItemTranslation = new MediaItemTranslation
            {
                MediaItemVersionId = mediaItemVersion.Id,
                Language = serviceModel.Language,
                Name = Path.GetFileNameWithoutExtension(serviceModel.File.FileName)
            };

            _context.MediaItemTranslations.Add(mediaItemTranslation.FillCommonProperties());
            _context.SaveChanges();

            await _mediaRepository.CreateFileAsync(mediaItemVersion.Id, serviceModel.OrganisationId.ToString(), serviceModel.File, serviceModel.File.FileName);

            return mediaItem.Id;
        }

        public async Task<Guid> UpdateFileAsync(UpdateMediaItemServiceModel serviceModel)
        {
            var existingMediaItemVersion = _context.MediaItemVersions.Where(x => x.MediaItemId == serviceModel.Id.Value && x.IsActive).OrderBy(o => o.Version);

            if (existingMediaItemVersion is not null)
            {
                var checksum = _checksumService.GetMd5(serviceModel.File);
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

                _context.MediaItemVersions.Add(mediaItemVersion.FillCommonProperties());
                
                var translations = _context.MediaItemTranslations.Where(x => x.MediaItemVersionId == existingMediaItemVersion.LastOrDefault().Id);

                foreach(var translation in translations)
                {
                    translation.MediaItemVersionId = mediaItemVersion.Id;
                    translation.LastModifiedDate = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                await _mediaRepository.CreateFileAsync(mediaItemVersion.Id, serviceModel.OrganisationId.ToString(), serviceModel.File, serviceModel.File.FileName);
            }

            return existingMediaItemVersion.FirstOrDefault().MediaItemId;
        }

        public MediaFileServiceModel GetFile(Guid? mediaId)
        {
            if (mediaId.HasValue)
            {
                var mediaItem = (from m in _context.MediaItems
                                 join mv in _context.MediaItemVersions on m.Id equals mv.MediaItemId
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

                if (mediaItem is not null)
                {
                    var file = _mediaRepository.GetFile(mediaItem.Folder, $"{mediaItem.VersionId}{mediaItem.Extension}");

                    if (file is not null)
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

            var pageIndex = model.PageIndex;
            var itemsPerPage = model.ItemsPerPage;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                pageIndex = Constants.DefaultPageIndex;
                itemsPerPage = Constants.MaxItemsPerPageLimit;
            }

            foreach (var id in model.Ids.Skip((pageIndex.Value - 1) * itemsPerPage.Value).Take(itemsPerPage.Value))
            {
                predicateBuilder = predicateBuilder.Or(x => x.Id == id);
            }

            var mediaItems = _context.MediaItems.Where(x => x.IsActive).Where(predicateBuilder).ToList();

            var mediaItemVersions = _context.MediaItemVersions.Where(x => mediaItems.Select(y => y.Id).Contains(x.MediaItemId)).OrderByDescending(x => x.CreatedDate).ToList();

            var mediaItemsTranslations = _context.MediaItemTranslations.Where(x => mediaItemVersions.Select(y => y.Id).Contains(x.MediaItemVersionId)).ToList();

            unorderedMediaItemsResults.AddRange(mediaItems.OrEmptyIfNull().Select(x => new MediaItemServiceModel
            {
                Id = x.Id,
                IsProtected = x.IsProtected,
                Filename = $"{mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Filename}{mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Extension}",
                Size = mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Size,
                MimeType = mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).MimeType,
                Extension= mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Extension,
                MediaItemVersionId = mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Id,
                Name = mediaItemsTranslations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Id && t.IsActive && t.Language == model.Language)?.Name ?? mediaItemsTranslations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Id && t.IsActive)?.Name,
                Description = mediaItemsTranslations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Id && t.IsActive && t.Language == model.Language)?.Description ?? mediaItemsTranslations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Id && t.IsActive)?.Description,
                MetaData = mediaItemsTranslations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Id && t.IsActive && t.Language == model.Language)?.Metadata ?? mediaItemsTranslations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Id && t.IsActive)?.Metadata,
                LastModifiedDate = x.LastModifiedDate,
                CreatedDate = x.CreatedDate
            }));

            foreach (var id in model.Ids.OrEmptyIfNull())
            {
                var mediaItemResult = unorderedMediaItemsResults.OrEmptyIfNull().FirstOrDefault(x => x.Id == id);

                if (mediaItemResult is not null)
                {
                    mediaItemsResults.Add(mediaItemResult);
                }
            }

            return new PagedResults<IEnumerable<MediaItemServiceModel>>(model.Ids.Count(), itemsPerPage.Value)
            { 
                   Data = mediaItemsResults
            };
        }

        public MediaItemServiceModel GetMediaItemById(GetMediaItemsByIdServiceModel model)
        {
            var mediaItem = _context.MediaItems.FirstOrDefault(x => x.Id == model.Id && x.IsActive);

            if (mediaItem is null)
            {
                throw new NotFoundException(_mediaResources.GetString("MediaNotFound"));
            }

            var mediaItemVersions = _context.MediaItemVersions.Where(x => x.MediaItemId == mediaItem.Id).OrderByDescending(x => x.CreatedDate).ToList();

            var translations = _context.MediaItemTranslations.Where(x => mediaItemVersions.Select(y => y.Id).Contains(x.MediaItemVersionId)).ToList();

            var mediaItemVersion = mediaItemVersions.FirstOrDefault();

            return new MediaItemServiceModel
            {
                Id = mediaItem.Id,
                IsProtected = mediaItem.IsProtected,
                Filename = $"{mediaItemVersion.Filename}{mediaItemVersion.Extension}",
                Size = mediaItemVersion.Size,
                MimeType = mediaItemVersion.MimeType,
                Extension = mediaItemVersion.Extension,
                MediaItemVersionId = mediaItemVersion.Id,
                Name = translations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersion.Id && t.IsActive && t.Language == model.Language)?.Name ?? translations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersion.Id && t.IsActive)?.Name,
                Description = translations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersion.Id && t.IsActive && t.Language == model.Language)?.Description ?? translations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersion.Id && t.IsActive)?.Description,
                MetaData = translations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersion.Id && t.IsActive && t.Language == model.Language)?.Metadata ?? translations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersion.Id && t.IsActive)?.Metadata,
                LastModifiedDate = mediaItem.LastModifiedDate,
                CreatedDate = mediaItem.CreatedDate
            };
        }

        public PagedResults<IEnumerable<MediaItemServiceModel>> Get(GetMediaItemsServiceModel serviceModel)
        {
            var mediaItems = _context.MediaItems.Where(x => x.IsActive && x.OrganisationId == serviceModel.OrganisationId.Value);

            if (string.IsNullOrWhiteSpace(serviceModel.SearchTerm) is false)
            {
                mediaItems = mediaItems.Where(x => x.Id.ToString() == serviceModel.SearchTerm || x.Versions.Any(x => x.Filename.StartsWith(serviceModel.SearchTerm) || x.Id.ToString() == serviceModel.SearchTerm));
            }

            mediaItems = mediaItems.ApplySort(serviceModel.OrderBy);

            PagedResults<IEnumerable<MediaItem>> pagedResults;

            if (serviceModel.PageIndex.HasValue is false || serviceModel.ItemsPerPage.HasValue is false)
            {
                mediaItems = mediaItems.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = mediaItems.PagedIndex(new Pagination(mediaItems.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = mediaItems.PagedIndex(new Pagination(mediaItems.Count(), serviceModel.ItemsPerPage.Value), serviceModel.PageIndex.Value);
            }

            var mediaItemVersions = _context.MediaItemVersions.Where(x => pagedResults.Data.Select(y => y.Id).Contains(x.MediaItemId)).OrderByDescending(x => x.CreatedDate).ToList();

            var translations = _context.MediaItemTranslations.Where(x => mediaItemVersions.Select(y => y.Id).Contains(x.MediaItemVersionId)).ToList();

            return new PagedResults<IEnumerable<MediaItemServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new MediaItemServiceModel
                {
                    Id = x.Id,
                    IsProtected = x.IsProtected,
                    Filename = $"{mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Filename}{mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Extension}",
                    Size = mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Size,
                    MimeType = mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).MimeType,
                    Extension = mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Extension,
                    MediaItemVersionId = mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Id,
                    Name = translations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Id && t.IsActive && t.Language == serviceModel.Language)?.Name ?? translations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Id && t.IsActive)?.Name,
                    Description = translations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Id && t.IsActive && t.Language == serviceModel.Language)?.Description ?? translations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Id && t.IsActive)?.Description,
                    MetaData = translations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Id && t.IsActive && t.Language == serviceModel.Language)?.Metadata ?? translations.FirstOrDefault(t => t.MediaItemVersionId == mediaItemVersions.FirstOrDefault(v => v.MediaItemId == x.Id).Id && t.IsActive)?.Metadata,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }

        public async Task DeleteAsync(DeleteFileServiceModel model)
        {
            var mediaItem = _context.MediaItems.FirstOrDefault(x => x.Id == model.MediaId.Value && x.IsActive);

            if (mediaItem is null)
            {
                throw new NotFoundException(_mediaResources.GetString("MediaNotFound"));
            }

            mediaItem.IsActive = false;
            mediaItem.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public MediaItemVerionsByIdServiceModel GetMediaItemVerionsById(GetMediaItemsByIdServiceModel model)
        {
            var mediaItemVersions = _context.MediaItemVersions
                .Where(x => x.MediaItemId == model.Id && x.IsActive)
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
                    CreatedDate = x.CreatedDate
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
            using var source = new ActivitySource(this.GetType().Name);

            var mediaVersion = _context.MediaItemVersions.OrderBy(o => o.Version).LastOrDefault(x => x.MediaItemId == model.Id.Value && x.IsActive);
            if (mediaVersion is not null)
            {
                var mediaVersionTranslation = _context.MediaItemTranslations.FirstOrDefault(x => x.MediaItemVersionId == mediaVersion.Id && x.Language == model.Language);
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

                    _context.Add(mediaItemTranslation.FillCommonProperties());
                }

                await _context.SaveChangesAsync();

                var message = new UpdatedFileIntegrationEvent
                {
                    FileId = model.Id.Value,
                    Name = model.Name
                };

                using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {message.GetType().Name}");
                _eventBus.Publish(message);
            }
        }

        public MediaFileServiceModel GetFileVersion(Guid? versionId)
        {
            if (versionId.HasValue)
            {
                var mediaItemVersion = (from mv in _context.MediaItemVersions
                                          join m in _context.MediaItems on mv.MediaItemId equals m.Id
                                          where mv.Id == versionId && m.IsProtected == false
                                          select new MediaFileItemServiceModel
                                          {
                                              Id = mv.Id,
                                              ContentType = mv.MimeType,
                                              Filename = mv.Filename,
                                              Extension = mv.Extension,
                                              Folder = mv.Folder
                                          }).FirstOrDefault();

                if (mediaItemVersion is not null)
                {
                    var file = _mediaRepository.GetFile(mediaItemVersion.Folder, $"{mediaItemVersion.Id}{mediaItemVersion.Extension}");

                    if (file is not null)
                    {
                        return new MediaFileServiceModel
                        {
                            Id = mediaItemVersion.Id,
                            Filename = $"{mediaItemVersion.Filename}{mediaItemVersion.Extension}",
                            ContentType = mediaItemVersion.ContentType,
                            File = file
                        };
                    }
                }
            }

            return default;
        }
        
        public async Task CreateFileChunkAsync(CreateFileChunkServiceModel model)
        {
            var directory = Path.Combine(
                    MediaConstants.Paths.TempPath,
                    model.OrganisationId.ToString());

            Directory.CreateDirectory(directory);

            var fileName = $"{model.UploadId}_{model.ChunkSumber}";
            var path = Path.Combine(directory, fileName);

            await using var fs = File.Create(path);

            await model.File.CopyToAsync(fs);
        }

        public async Task<Guid> CreateFileFromChunksAsync(CreateMediaItemFromChunksServiceModel model)
        {
            var directory = Path.Combine(
                    MediaConstants.Paths.TempPath,
                    model.OrganisationId.ToString());

            var searchPattern = $"{model.UploadId}_*";

            if (!Directory.Exists(directory))
                throw new CustomException(_mediaResources.GetString("ChunkDirectoryNotFound"), (int)HttpStatusCode.UnprocessableEntity);

            string[] filePaths = Directory.GetFiles(directory, searchPattern)
                .OrderBy(p =>
                {
                    var name = Path.GetFileName(p);
                    var parts = name.Split('_');

                    return int.Parse(parts[1]);
                })
                .ToArray();

            if (!filePaths.Any())
                throw new CustomException(_mediaResources.GetString("NoChunksFound"), (int)HttpStatusCode.UnprocessableEntity);

            string newPath = Path.Combine(directory, model.Filename);

            foreach (var filePath in filePaths)
            {
                MergeChunks(newPath, filePath);
            }

            await using var stream = new FileStream(newPath, FileMode.Open, FileAccess.Read, FileShare.Read);

            var formFile = new FormFile(stream, 0, stream.Length, model.Filename, model.Filename);

            var id = await this.CreateFileAsync(new CreateMediaItemServiceModel
            {
                File = formFile,
                OrganisationId = model.OrganisationId,
                Language = model.Language,
                Username = model.Username
            });

            File.Delete(newPath);

            return id;
        }

        public async Task<Guid> UpdateFileFromChunksAsync(UpdateMediaItemFromChunksServiceModel model)
        {
            var directory = Path.Combine(
                    MediaConstants.Paths.TempPath,
                    model.OrganisationId?.ToString());

            var searchPattern = $"{model.UploadId}_*";

            if (!Directory.Exists(directory))
                throw new CustomException(_mediaResources.GetString("ChunkDirectoryNotFound"), (int)HttpStatusCode.UnprocessableEntity);

            string[] filePaths = Directory.GetFiles(directory, searchPattern)
                .OrderBy(p =>
                {
                    var name = Path.GetFileName(p);
                    var parts = name.Split('_');

                    return int.Parse(parts[1]);
                })
                .ToArray();

            if (!filePaths.Any())
                throw new CustomException(_mediaResources.GetString("NoChunksFound"), (int)HttpStatusCode.UnprocessableEntity);

            string newPath = Path.Combine(directory, model.Filename);

            foreach (var filePath in filePaths)
            {
                MergeChunks(newPath, filePath);
            }

            await using var stream = new FileStream(newPath, FileMode.Open, FileAccess.Read, FileShare.Read);

            var formFile = new FormFile(stream, 0, stream.Length, model.Filename, model.Filename);

            var id = await this.UpdateFileAsync(new UpdateMediaItemServiceModel
            {
                File = formFile,
                Id = model.Id,
                Language = model.Language,
                OrganisationId = model.OrganisationId,
                Username = model.Username
            });

            File.Delete(newPath);

            return id;
        }

        private static void MergeChunks(string targetPath, string sourcePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);

            using (var fs1 = new FileStream(targetPath, FileMode.Append, FileAccess.Write, FileShare.None))
            using (var fs2 = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs2.CopyTo(fs1);
            }

            File.Delete(sourcePath);
        }
    }
}
