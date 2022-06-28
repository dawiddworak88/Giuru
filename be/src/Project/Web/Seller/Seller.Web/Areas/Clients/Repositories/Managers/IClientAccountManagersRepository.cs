using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.Managers
{
    public interface IClientAccountManagersRepository
    {
        Task<Guid> SaveAsync(string token, string language, Guid? id, string firstName, string lastName, string email, string phoneNumber);
        Task<PagedResults<IEnumerable<ClientAccountManager>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<ClientAccountManager> GetAsync(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<IEnumerable<ClientAccountManager>> GetAsync(string token, string language);
    }
}
