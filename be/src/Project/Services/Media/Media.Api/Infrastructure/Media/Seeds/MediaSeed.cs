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
        public static void SeedHeaders(MediaContext context, string storageConnectionString, IChecksumService checksumService)
        {
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Headers.LogoMediaId, MediaConstants.Headers.LogoMediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.Headers.LogoMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Headers.FaviconMediaId, MediaConstants.Headers.FaviconMediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.Headers.FaviconMediaUrl);
        }

        public static void SeedHeroSliderItems(MediaContext context, string storageConnectionString, IChecksumService checksumService)
        {
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.BoxspringsMediaId, MediaConstants.HeroSliderItems.BoxspringsMediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.BoxspringsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Boxsprings1600x400MediaId, MediaConstants.HeroSliderItems.Boxsprings1600x400MediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.Boxsprings1600x400MediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Boxsprings1024x400MediaId, MediaConstants.HeroSliderItems.Boxsprings1024x400MediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.Boxsprings1024x400MediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Boxsprings414x286MediaId, MediaConstants.HeroSliderItems.Boxsprings414x286MediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.Boxsprings414x286MediaUrl);

            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.ChairsMediaId, MediaConstants.HeroSliderItems.ChairsMediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.ChairsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Chairs1600x400MediaId, MediaConstants.HeroSliderItems.Chairs1600x400MediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.Chairs1600x400MediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Chairs1024x400MediaId, MediaConstants.HeroSliderItems.Chairs1024x400MediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.Chairs1024x400MediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Chairs414x286MediaId, MediaConstants.HeroSliderItems.Chairs414x286MediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.Chairs414x286MediaUrl);

            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.CornersMediaId, MediaConstants.HeroSliderItems.CornersMediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.CornersMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Corners1600x400MediaId, MediaConstants.HeroSliderItems.Corners1600x400MediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.Corners1600x400MediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Corners1024x400MediaId, MediaConstants.HeroSliderItems.Corners1024x400MediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.Corners1024x400MediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Corners414x286MediaId, MediaConstants.HeroSliderItems.Corners414x286MediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.Corners414x286MediaUrl);

            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.SetsMediaId, MediaConstants.HeroSliderItems.SetsMediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.SetsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Sets1600x400MediaId, MediaConstants.HeroSliderItems.Sets1600x400MediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.Sets1600x400MediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Sets1024x400MediaId, MediaConstants.HeroSliderItems.Sets1024x400MediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.Sets1024x400MediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.Sets414x286MediaId, MediaConstants.HeroSliderItems.Sets414x286MediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.HeroSliderItems.Sets414x286MediaUrl);
        }

        public static void SeedCategories(MediaContext context, string storageConnectionString, IChecksumService checksumService)
        {
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.CouchesMediaId, MediaConstants.Categories.CouchesMediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.Categories.CouchesMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.SectionalsMediaId, MediaConstants.Categories.SectionalsMediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.Categories.SectionalsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.CoffeeTablesMediaId, MediaConstants.Categories.CoffeeTablesMediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.Categories.CoffeeTablesMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.ChairsMediaId, MediaConstants.Categories.ChairsMediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.Categories.ChairsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.PoufsMediaId, MediaConstants.Categories.PoufsMediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.Categories.PoufsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.SetsMediaId, MediaConstants.Categories.SetsMediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.Categories.SetsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.BedsMediaId, MediaConstants.Categories.BedsMediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.Categories.BedsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.MattressesMediaId, MediaConstants.Categories.MattressesMediaVersionId, MediaConstants.General.DefaultOrganisationId, MediaConstants.Categories.MattressesMediaUrl);
        }

        private static void SeedMedia(MediaContext context, IChecksumService checksumService, string storageConnectionString, Guid mediaId, Guid mediaVersionId, Guid? organisationId, string mediaUrl)
        {
            if (!context.MediaItems.Any(x => x.Id == mediaId))
            {
                var container = new BlobContainerClient(storageConnectionString, MediaConstants.General.ContainerName);

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
    }
}
