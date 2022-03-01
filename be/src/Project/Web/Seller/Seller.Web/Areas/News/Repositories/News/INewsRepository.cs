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
        Task<Guid> SaveAsync(
            string token, string language, Guid? id, Guid? categoryId, Guid? heroImageId, string title, string description, 
            string content, bool isNew, bool isPublished, IEnumerable<Guid> images, IEnumerable<Guid> files);
    }
}
