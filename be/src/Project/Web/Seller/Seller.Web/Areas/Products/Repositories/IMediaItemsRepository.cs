using Seller.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.Repositories
{
    public interface IMediaItemsRepository
    {
        Task<IEnumerable<MediaItem>> GetAllMediaItemsAsync(string token, string language, string mediaItemIds, int pageIndex, int itemsPerPage);
        Task<MediaItem> GetMediaItemAsync(string token, string language, Guid id);
    }
}
