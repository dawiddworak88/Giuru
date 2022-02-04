using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Media.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.Repositories.Media
{
    public interface IMediaRepository
    {
        Task<PagedResults<IEnumerable<MediaItem>>> GetMediaItemsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<MediaItemVersions> GetMediaItemVersionsAsync(Guid? mediaId, string token, string language);
        Task UpdateMediaItemVersionAsync(Guid? mediaId, string name, string description, string token, string language);
        Task DeleteAsync(string token, string language, Guid? mediaId);
    }
}
