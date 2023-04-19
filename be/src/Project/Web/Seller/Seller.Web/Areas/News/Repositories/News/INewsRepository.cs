using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.News.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.News.Repositories.News
{
    public interface INewsRepository
    {
        Task<PagedResults<IEnumerable<NewsItem>>> GetNewsItemsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<NewsItem> GetAsync(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<Guid> SaveAsync(
            string token, string language, Guid? id, Guid? thumbnailImageId, Guid? categoryId, Guid? previewImageId, 
            string title, string description, string content, bool isPublished, IEnumerable<Guid> files, IEnumerable<Guid> groupIds);
    }
}
