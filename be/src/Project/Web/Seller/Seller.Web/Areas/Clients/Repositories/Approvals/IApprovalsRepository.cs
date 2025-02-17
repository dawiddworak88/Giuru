using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.Approvals
{
    public interface IApprovalsRepository
    {
        Task<PagedResults<IEnumerable<Approval>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task DeleteAsync(string token, string language, Guid? id);
        Task SaveAsync(string token, string language, Guid? id, string name, string description);
        Task<Approval> GetAsync(string token, string language, Guid? id);
    }
}
