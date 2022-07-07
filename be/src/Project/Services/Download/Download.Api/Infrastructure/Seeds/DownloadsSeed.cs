using Download.Api.Definitions;
using Foundation.GenericRepository.Extensions;
using System;
using System.Linq;

namespace Download.Api.Infrastructure.Seeds
{
    public static class DownloadsSeed
    {
        public static void SeedDownloads(DownloadContext context)
        {
            SeedDownload(context, DownloadConstants.Downloads.CollectionDownloads, DownloadConstants.Categories.CollectionsCategory, 100);
            SeedDownload(context, DownloadConstants.Downloads.MarketingDownloads, DownloadConstants.Categories.MarketingCategory, 90);
            SeedDownload(context, DownloadConstants.Downloads.TechnicalDownloads, DownloadConstants.Categories.TechnicalCategory, 80);
        }

        private static void SeedDownload(DownloadContext context, Guid id, Guid categoryId, int? order)
        {
            if (!context.Downloads.Any(x => x.Id == id))
            {
                var download = new Entities.Downloads.Download
                {
                    Id = id,
                    CategoryId = categoryId,
                    Order = order
                };

                context.Downloads.Add(download.FillCommonProperties());
                context.SaveChanges();
            }
        }
    }
}
