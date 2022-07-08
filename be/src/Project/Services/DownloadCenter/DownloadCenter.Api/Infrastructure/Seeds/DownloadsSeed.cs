using DownloadCenter.Api.Definitions;
using Foundation.GenericRepository.Extensions;
using System;
using System.Linq;

namespace DownloadCenter.Api.Infrastructure.Seeds
{
    public static class DownloadsSeed
    {
        public static void SeedDownloads(DownloadContext context)
        {
            SeedDownload(context, DownloadCenterConstants.DownloadCenter.CollectionDownloads, DownloadCenterConstants.Categories.CollectionsCategory, 100);
            SeedDownload(context, DownloadCenterConstants.DownloadCenter.MarketingDownloads, DownloadCenterConstants.Categories.MarketingCategory, 90);
            SeedDownload(context, DownloadCenterConstants.DownloadCenter.TechnicalDownloads, DownloadCenterConstants.Categories.TechnicalCategory, 80);
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
