using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Clients.DomainModels;

namespace Seller.Web.Areas.Clients.Repositories
{
    public interface IClientsRepository
    {
        Task<Client> GetClientAsync(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<Guid> SaveAsync(string token, string language, Guid? id, string name, string email, string communicationLanguage);
        Task<PagedResults<IEnumerable<Client>>> GetClientsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage);
        Task<IEnumerable<Client>> GetAllClientsAsync(string token, string language);
    }
}
