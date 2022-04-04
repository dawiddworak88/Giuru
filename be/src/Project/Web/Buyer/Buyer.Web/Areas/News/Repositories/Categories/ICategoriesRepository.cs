using Buyer.Web.Areas.News.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.Repositories.Categories
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(string token, string language);
    }
}
