using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Seller.Web.Areas.Global.DomainModels;

namespace Seller.Web.Areas.Global.Repositories
{
    public interface ICountriesRepository
    {
        Task SaveAsync(string token, string language, Guid? id, string name);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<PagedResults<IEnumerable<Country>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<IEnumerable<Country>> GetAsync(string token, string language);
        Task<Country> GetAsync(string token, string language, Guid? id);
    }
}
