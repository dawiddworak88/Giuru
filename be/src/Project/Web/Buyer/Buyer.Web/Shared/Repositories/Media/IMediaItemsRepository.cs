using Buyer.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Media
{
    public interface IMediaItemsRepository
    {
        Task<MediaItem> GetMediaItemAsync(string token, string language, Guid id);
        Task<IEnumerable<MediaItem>> GetMediaItemsAsync(string token, string language, IEnumerable<Guid> ids, int pageIndex, int itemsPerPage);
    }
}
