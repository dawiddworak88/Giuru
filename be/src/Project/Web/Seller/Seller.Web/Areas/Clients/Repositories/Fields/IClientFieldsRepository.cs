using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.Fields
{
    public interface IClientFieldsRepository
    {
        Task<PagedResults<IEnumerable<ClientField>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<Guid> SaveAsync(string token, string language, Guid? id, string name, string type);
        Task<ClientField> GetAsync(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language, Guid? id);
    }
}
