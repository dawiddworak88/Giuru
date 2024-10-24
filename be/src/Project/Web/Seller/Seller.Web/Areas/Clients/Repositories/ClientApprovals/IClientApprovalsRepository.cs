using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.ClientApprovals
{
    public interface IClientApprovalsRepository
    {
        Task<PagedResults<IEnumerable<ClientApproval>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task DeleteAsync(string token, string language, Guid? id);
        Task SaveAsync(string token, string language, Guid? id, string name);
        Task<ClientApproval> GetAsync(string token, string language, Guid? id);
    }
}
