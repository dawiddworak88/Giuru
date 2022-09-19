using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.Roles
{
    public interface IClientRolesRepository
    {
        Task<Guid> SaveAsync(string token, string language, Guid? id, string name);
        Task<PagedResults<IEnumerable<ClientRole>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<ClientRole> GetAsync(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language, Guid? id);
    }
}
