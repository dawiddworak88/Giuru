using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Global.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Global.Repositories
{
    public interface ICurrenciesRepository
    {
        Task SaveAsync(string token, string language, Guid? id, string currencyCode, string symbol, string name);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<PagedResults<IEnumerable<Currency>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<IEnumerable<Currency>> GetAsync(string token, string language, string orderBy);
        Task<Currency> GetAsync(string token, string language, Guid? id);
    }
}
