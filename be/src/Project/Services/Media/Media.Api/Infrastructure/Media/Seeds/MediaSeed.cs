using Azure.Storage.Blobs;
using Foundation.GenericRepository.Helpers;
using Media.Api.Definitions;
using Media.Api.Infrastructure.Media.Entities;
using MimeMapping;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace Media.Api.Infrastructure.Media.Seeds
{
    public static class MediaSeed
    {
        public static void SeedCategories(MediaContext context, string storageConnectionString)
        {
            SeedMedia(context, storageConnectionString, MediaConstants.Categories.CouchesMediaId, MediaConstants.Categories.CouchesMediaVersionId, MediaConstants.Categories.CouchesMediaUrl);
            SeedMedia(context, storageConnectionString, MediaConstants.Categories.SectionalsMediaId, MediaConstants.Categories.SectionalsMediaVersionId, MediaConstants.Categories.SectionalsMediaUrl);
            SeedMedia(context, storageConnectionString, MediaConstants.Categories.CoffeeTablesMediaId, MediaConstants.Categories.CoffeeTablesMediaVersionId, MediaConstants.Categories.CoffeeTablesMediaUrl);
            SeedMedia(context, storageConnectionString, MediaConstants.Categories.TvStandsMediaId, MediaConstants.Categories.TvStandsMediaVersionId, MediaConstants.Categories.TvStandsMediaUrl);
            SeedMedia(context, storageConnectionString, MediaConstants.Categories.WallUnitsMediaId, MediaConstants.Categories.WallUnitsMediaVersionId, MediaConstants.Categories.WallUnitsMediaUrl);
            SeedMedia(context, storageConnectionString, MediaConstants.Categories.ChairsMediaId, MediaConstants.Categories.ChairsMediaVersionId, MediaConstants.Categories.ChairsMediaUrl);
            SeedMedia(context, storageConnectionString, MediaConstants.Categories.PoufsMediaId, MediaConstants.Categories.PoufsMediaVersionId, MediaConstants.Categories.PoufsMediaUrl);

            SeedMedia(context, storageConnectionString, MediaConstants.Categories.BedsMediaId, MediaConstants.Categories.BedsMediaVersionId, MediaConstants.Categories.BedsMediaUrl);
            SeedMedia(context, storageConnectionString, MediaConstants.Categories.WardrobesMediaId, MediaConstants.Categories.WardrobesMediaVersionId, MediaConstants.Categories.WardrobesMediaUrl);
            SeedMedia(context, storageConnectionString, MediaConstants.Categories.ChestsMediaId, MediaConstants.Categories.ChestsMediaVersionId, MediaConstants.Categories.ChestsMediaUrl);
            SeedMedia(context, storageConnectionString, MediaConstants.Categories.MattressesMediaId, MediaConstants.Categories.MattressesMediaVersionId, MediaConstants.Categories.MattressesMediaUrl);

            SeedMedia(context, storageConnectionString, MediaConstants.Categories.DiningTablesSeatingMediaId, MediaConstants.Categories.DiningTablesSeatingMediaVersionId, MediaConstants.Categories.DiningTablesSeatingMediaUrl);

            SeedMedia(context, storageConnectionString, MediaConstants.Categories.BathroomVanitiesCabinetsMediaId, MediaConstants.Categories.BathroomVanitiesCabinetsMediaVersionId, MediaConstants.Categories.BathroomVanitiesCabinetsMediaUrl);

            SeedMedia(context, storageConnectionString, MediaConstants.Categories.KidsBedsMediaId, MediaConstants.Categories.KidsBedsMediaVersionId, MediaConstants.Categories.KidsBedsMediaUrl);
            SeedMedia(context, storageConnectionString, MediaConstants.Categories.KidsBunkBedsMediaId, MediaConstants.Categories.KidsBunkBedsMediaVersionId, MediaConstants.Categories.KidsBunkBedsMediaUrl);
            SeedMedia(context, storageConnectionString, MediaConstants.Categories.KidsDesksMediaId, MediaConstants.Categories.KidsDesksMediaVersionId, MediaConstants.Categories.KidsDesksMediaUrl);
        }

        private static void SeedMedia(MediaContext context, string storageConnectionString, Guid mediaId, Guid mediaVersionId, string mediaUrl)
        {
            if (!context.MediaItems.Any(x => x.Id == mediaId))
            {
                var container = new BlobContainerClient(storageConnectionString, MediaConstants.General.ContainerName);

                container.CreateIfNotExists();

                var blob = container.GetBlobClient($"{mediaVersionId}{Path.GetExtension(mediaUrl)}");

                if (!blob.Exists())
                {
                    using (var client = new WebClient())
                    using (var stream = new MemoryStream(File.ReadAllBytes(mediaUrl)))
                    {
                        blob.Upload(stream);
                    }
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
                    Filename = Path.GetFileNameWithoutExtension(mediaUrl),
                    Extension = Path.GetExtension(mediaUrl),
                    Folder = MediaConstants.General.ContainerName,
                    MimeType = MimeUtility.GetMimeMapping(Path.GetExtension(mediaUrl)),
                    Size = blob.GetProperties().Value.ContentLength,
                    Version = 1
                };

                context.MediaItemVersions.Add(EntitySeedHelper.SeedEntity(mediaItemVersion));

                context.SaveChanges();
            }
        }
    }
}
