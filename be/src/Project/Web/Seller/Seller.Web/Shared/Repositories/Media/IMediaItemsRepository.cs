using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Shared.Repositories.Media
{
    public interface IMediaItemsRepository
    {
        Task<IEnumerable<MediaItem>> GetAllMediaItemsAsync(string token, string language, string mediaItemIds, int pageIndex, int itemsPerPage);
        Task<MediaItem> GetMediaItemAsync(string token, string language, Guid id);
        Task<PagedResults<IEnumerable<MediaItem>>> GetMediaItemsAsync(IEnumerable<Guid> ids, string language, int pageIndex, int itemsPerPage, string token);
    }
}
