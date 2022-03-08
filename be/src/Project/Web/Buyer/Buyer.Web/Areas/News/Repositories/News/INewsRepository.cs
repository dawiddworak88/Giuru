using Buyer.Web.Areas.News.DomainModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.Repositories.News
{
    public interface INewsRepository
    {
        Task<PagedResults<IEnumerable<NewsItem>>> GetNewsItemsAsync(string token, string language, int pageIndex, int itemsPerPage, string orderBy);
        Task<NewsItem> GetNewsItemAsync(string token, string language, Guid? id);
    }
}
