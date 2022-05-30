using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories
{
    public interface IClientGroupsRepository
    {
        Task<Guid> SaveAsync(string token, string language, Guid? id, string name);
        Task<PagedResults<IEnumerable<ClientGroup>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<ClientGroup> GetAsync(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<IEnumerable<ClientGroup>> GetAsync(string token, string language);
    }
}
