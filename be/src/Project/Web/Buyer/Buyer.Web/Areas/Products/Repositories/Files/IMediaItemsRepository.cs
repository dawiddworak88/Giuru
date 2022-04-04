using Buyer.Web.Areas.Products.DomainModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.Files
{
    public interface IMediaItemsRepository
    {
        Task<PagedResults<IEnumerable<MediaItem>>> GetMediaItemsAsync(IEnumerable<Guid> ids, string language, int pageIndex, int itemsPerPage, string token);
        Task<MediaItem> GetMediaItemAsync(string token, string language, Guid id);
    }
}
