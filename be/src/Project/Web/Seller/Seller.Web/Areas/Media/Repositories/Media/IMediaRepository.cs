using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Media.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.Repositories.Media
{
    public interface IMediaRepository
    {
        Task<PagedResults<IEnumerable<MediaItem>>> GetMediaItemsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
    }
}
