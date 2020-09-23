using Azure.Storage.Blobs;
using Foundation.GenericRepository.Helpers;
using Media.Api.Definitions;
using Media.Api.Infrastructure.Media.Entities;
using Media.Api.Shared.Checksums;
using MimeMapping;
using System;
using System.IO;
using System.Linq;

namespace Media.Api.Infrastructure.Media.Seeds
{
    public static class MediaSeed
    {
        public static void SeedHeroSliderItems(MediaContext context, string storageConnectionString, IChecksumService checksumService)
        {
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.LivingRoomMediaId, MediaConstants.HeroSliderItems.LivingRoomMediaVersionId, MediaConstants.HeroSliderItems.LivingRoomMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.BedroomMediaId, MediaConstants.HeroSliderItems.BedroomMediaVersionId, MediaConstants.HeroSliderItems.BedroomMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.HeroSliderItems.KidsRoomMediaId, MediaConstants.HeroSliderItems.KidsRoomMediaVersionId, MediaConstants.HeroSliderItems.KidsRoomMediaUrl);
        }

        public static void SeedCategories(MediaContext context, string storageConnectionString, IChecksumService checksumService)
        {
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.CouchesMediaId, MediaConstants.Categories.CouchesMediaVersionId, MediaConstants.Categories.CouchesMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.SectionalsMediaId, MediaConstants.Categories.SectionalsMediaVersionId, MediaConstants.Categories.SectionalsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.CoffeeTablesMediaId, MediaConstants.Categories.CoffeeTablesMediaVersionId, MediaConstants.Categories.CoffeeTablesMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.TvStandsMediaId, MediaConstants.Categories.TvStandsMediaVersionId, MediaConstants.Categories.TvStandsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.WallUnitsMediaId, MediaConstants.Categories.WallUnitsMediaVersionId, MediaConstants.Categories.WallUnitsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.ChairsMediaId, MediaConstants.Categories.ChairsMediaVersionId, MediaConstants.Categories.ChairsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.PoufsMediaId, MediaConstants.Categories.PoufsMediaVersionId, MediaConstants.Categories.PoufsMediaUrl);

            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.BedsMediaId, MediaConstants.Categories.BedsMediaVersionId, MediaConstants.Categories.BedsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.WardrobesMediaId, MediaConstants.Categories.WardrobesMediaVersionId, MediaConstants.Categories.WardrobesMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.ChestsMediaId, MediaConstants.Categories.ChestsMediaVersionId, MediaConstants.Categories.ChestsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.MattressesMediaId, MediaConstants.Categories.MattressesMediaVersionId, MediaConstants.Categories.MattressesMediaUrl);

            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.DiningTablesSeatingMediaId, MediaConstants.Categories.DiningTablesSeatingMediaVersionId, MediaConstants.Categories.DiningTablesSeatingMediaUrl);

            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.BathroomVanitiesCabinetsMediaId, MediaConstants.Categories.BathroomVanitiesCabinetsMediaVersionId, MediaConstants.Categories.BathroomVanitiesCabinetsMediaUrl);

            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.KidsBedsMediaId, MediaConstants.Categories.KidsBedsMediaVersionId, MediaConstants.Categories.KidsBedsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.KidsBunkBedsMediaId, MediaConstants.Categories.KidsBunkBedsMediaVersionId, MediaConstants.Categories.KidsBunkBedsMediaUrl);
            SeedMedia(context, checksumService, storageConnectionString, MediaConstants.Categories.KidsDesksMediaId, MediaConstants.Categories.KidsDesksMediaVersionId, MediaConstants.Categories.KidsDesksMediaUrl);
        }

        private static void SeedMedia(MediaContext context, IChecksumService checksumService, string storageConnectionString, Guid mediaId, Guid mediaVersionId, string mediaUrl)
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
                        IsProtected = false
                    };

                    context.MediaItems.Add(EntitySeedHelper.SeedEntity(mediaItem));

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

                    context.MediaItemVersions.Add(EntitySeedHelper.SeedEntity(mediaItemVersion));

                    context.SaveChanges();
                }
            }
        }
    }
}
