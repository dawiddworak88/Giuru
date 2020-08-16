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
            SeedMedia(context, storageConnectionString, MediaConstants.Categories.SectionalsMediaId, MediaConstants.Categories.SectionalsMediaVersionId, MediaConstants.Categories.SectionalsMediaUrl);
        }

        private static void SeedMedia(MediaContext context, string storageConnectionString, Guid mediaId, Guid mediaVersionId, string mediaUrl)
        {
            if (!context.MediaItems.Any(x => x.Id == mediaId))
            {
                var request = (HttpWebRequest)WebRequest.Create(mediaUrl);
                var response = (HttpWebResponse)request.GetResponse();
                var inputStream = response.GetResponseStream();

                var container = new BlobContainerClient(storageConnectionString, MediaConstants.General.ContainerName);

                container.CreateIfNotExists();

                var blob = container.GetBlobClient($"{mediaVersionId}{Path.GetExtension(mediaUrl)}");

                if (!blob.Exists())
                {
                    blob.Upload(inputStream);
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
