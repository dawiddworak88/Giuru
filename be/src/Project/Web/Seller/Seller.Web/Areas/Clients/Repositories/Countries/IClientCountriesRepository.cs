using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Seller.Web.Areas.Clients.DomainModels;

namespace Seller.Web.Areas.Clients.Repositories.Countries
{
    public interface IClientCountriesRepository
    {
        Task SaveAsync(string token, string language, Guid? id, string name);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<PagedResults<IEnumerable<ClientCountry>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<ClientCountry> GetAsync(string token, string language, Guid? id);
    }
}
