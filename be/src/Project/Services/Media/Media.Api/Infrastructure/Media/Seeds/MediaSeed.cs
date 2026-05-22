using Azure.Storage.Blobs;
using Foundation.GenericRepository.Extensions;
using Media.Api.Definitions;
using Media.Api.Infrastructure.Media.Entities;
using Media.Api.Services.Checksums;
using MimeMapping;
using System;
using System.IO;
using System.Linq;

namespace Media.Api.Infrastructure.Media.Seeds
{
    public static class MediaSeed
    {
        private static readonly BlobClientOptions LegacyBlobClientOptions = new BlobClientOptions(BlobClientOptions.ServiceVersion.V2024_05_04);

        public static void SeedHeaders(MediaContext context, string storageConnectionString, Guid? organisationId, IChecksumService checksumService, bool useLegacyBlobServiceApiVersion)
        {
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Headers.LogoMediaId, MediaConstants.Headers.LogoMediaVersionId, organisationId, MediaConstants.Headers.LogoMediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Headers.FaviconMediaId, MediaConstants.Headers.FaviconMediaVersionId, organisationId, MediaConstants.Headers.FaviconMediaUrl, useLegacyBlobServiceApiVersion);
        }

        public static void SeedHeroSliderItems(MediaContext context, string storageConnectionString, Guid? organisationId, IChecksumService checksumService, bool useLegacyBlobServiceApiVersion)
        {
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.BoxspringsMediaId, MediaConstants.HeroSliderItems.BoxspringsMediaVersionId, organisationId, MediaConstants.HeroSliderItems.BoxspringsMediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Boxsprings1600x400MediaId, MediaConstants.HeroSliderItems.Boxsprings1600x400MediaVersionId, organisationId, MediaConstants.HeroSliderItems.Boxsprings1600x400MediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Boxsprings1024x400MediaId, MediaConstants.HeroSliderItems.Boxsprings1024x400MediaVersionId, organisationId, MediaConstants.HeroSliderItems.Boxsprings1024x400MediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Boxsprings414x286MediaId, MediaConstants.HeroSliderItems.Boxsprings414x286MediaVersionId, organisationId, MediaConstants.HeroSliderItems.Boxsprings414x286MediaUrl, useLegacyBlobServiceApiVersion);

            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.ChairsMediaId, MediaConstants.HeroSliderItems.ChairsMediaVersionId, organisationId, MediaConstants.HeroSliderItems.ChairsMediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Chairs1600x400MediaId, MediaConstants.HeroSliderItems.Chairs1600x400MediaVersionId, organisationId, MediaConstants.HeroSliderItems.Chairs1600x400MediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Chairs1024x400MediaId, MediaConstants.HeroSliderItems.Chairs1024x400MediaVersionId, organisationId, MediaConstants.HeroSliderItems.Chairs1024x400MediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Chairs414x286MediaId, MediaConstants.HeroSliderItems.Chairs414x286MediaVersionId, organisationId, MediaConstants.HeroSliderItems.Chairs414x286MediaUrl, useLegacyBlobServiceApiVersion);

            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.CornersMediaId, MediaConstants.HeroSliderItems.CornersMediaVersionId, organisationId, MediaConstants.HeroSliderItems.CornersMediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Corners1600x400MediaId, MediaConstants.HeroSliderItems.Corners1600x400MediaVersionId, organisationId, MediaConstants.HeroSliderItems.Corners1600x400MediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Corners1024x400MediaId, MediaConstants.HeroSliderItems.Corners1024x400MediaVersionId, organisationId, MediaConstants.HeroSliderItems.Corners1024x400MediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Corners414x286MediaId, MediaConstants.HeroSliderItems.Corners414x286MediaVersionId, organisationId, MediaConstants.HeroSliderItems.Corners414x286MediaUrl, useLegacyBlobServiceApiVersion);

            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.SetsMediaId, MediaConstants.HeroSliderItems.SetsMediaVersionId, organisationId, MediaConstants.HeroSliderItems.SetsMediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Sets1600x400MediaId, MediaConstants.HeroSliderItems.Sets1600x400MediaVersionId, organisationId, MediaConstants.HeroSliderItems.Sets1600x400MediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Sets1024x400MediaId, MediaConstants.HeroSliderItems.Sets1024x400MediaVersionId, organisationId, MediaConstants.HeroSliderItems.Sets1024x400MediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Sets414x286MediaId, MediaConstants.HeroSliderItems.Sets414x286MediaVersionId, organisationId, MediaConstants.HeroSliderItems.Sets414x286MediaUrl, useLegacyBlobServiceApiVersion);
        }

        public static void SeedCategories(MediaContext context, string storageConnectionString, Guid? organisationId, IChecksumService checksumService, bool useLegacyBlobServiceApiVersion)
        {
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.CouchesMediaId, MediaConstants.Categories.CouchesMediaVersionId, organisationId, MediaConstants.Categories.CouchesMediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.SectionalsMediaId, MediaConstants.Categories.SectionalsMediaVersionId, organisationId, MediaConstants.Categories.SectionalsMediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.CoffeeTablesMediaId, MediaConstants.Categories.CoffeeTablesMediaVersionId, organisationId, MediaConstants.Categories.CoffeeTablesMediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.ChairsMediaId, MediaConstants.Categories.ChairsMediaVersionId, organisationId, MediaConstants.Categories.ChairsMediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.PoufsMediaId, MediaConstants.Categories.PoufsMediaVersionId, organisationId, MediaConstants.Categories.PoufsMediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.SetsMediaId, MediaConstants.Categories.SetsMediaVersionId, organisationId, MediaConstants.Categories.SetsMediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.BedsMediaId, MediaConstants.Categories.BedsMediaVersionId, organisationId, MediaConstants.Categories.BedsMediaUrl, useLegacyBlobServiceApiVersion);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.MattressesMediaId, MediaConstants.Categories.MattressesMediaVersionId, organisationId, MediaConstants.Categories.MattressesMediaUrl, useLegacyBlobServiceApiVersion);
        }

        private static void SeedMedia(MediaContext context, IChecksumService checksumService, string storageConnectionString, Guid mediaId, Guid mediaVersionId, Guid? organisationId, string mediaUrl, bool useLegacyBlobServiceApiVersion)
        {
            if (!context.MediaItems.Any(x => x.Id == mediaId))
            {
                var container = CreateContainerClient(storageConnectionString, MediaConstants.General.ContainerName, useLegacyBlobServiceApiVersion);

                container.CreateIfNotExists();

                var blob = container.GetBlobClient($"{mediaVersionId}{Path.GetExtension(mediaUrl)}");

                using (var stream = new MemoryStream(File.ReadAllBytes(mediaUrl)))
                {
                    if (!blob.Exists())
                    {
                        blob.Upload(stream);
                    }

                    var mediaItem = new MediaItem
                    {
                        Id = mediaId,
                        OrganisationId = organisationId,
                        IsProtected = false
                    };

                    context.MediaItems.Add(mediaItem.FillCommonProperties());

                    var mediaItemVersion = new MediaItemVersion
                    {
                        Id = mediaVersionId,
                        MediaItemId = mediaId,
                        Checksum = checksumService.GetMd5(stream),
                        Filename = Path.GetFileNameWithoutExtension(mediaUrl),
                        Extension = Path.GetExtension(mediaUrl),
                        Folder = MediaConstants.General.ContainerName,
                        MimeType = MimeUtility.GetMimeMapping(Path.GetExtension(mediaUrl)),
                        Size = blob.GetProperties().Value.ContentLength,
                        CreatedBy = "system",
                        Version = 1
                    };

                    context.MediaItemVersions.Add(mediaItemVersion.FillCommonProperties());

                    context.SaveChanges();
                }
            }
        }

        private static BlobContainerClient CreateContainerClient(string storageConnectionString, string containerName, bool useLegacyBlobServiceApiVersion)
        {
            if (string.IsNullOrWhiteSpace(storageConnectionString) || useLegacyBlobServiceApiVersion is false)
            {
                return new BlobContainerClient(storageConnectionString, containerName);
            }

            return new BlobContainerClient(storageConnectionString, containerName, LegacyBlobClientOptions);
        }
    }
}
