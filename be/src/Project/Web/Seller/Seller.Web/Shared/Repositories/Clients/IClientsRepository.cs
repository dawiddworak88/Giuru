using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Clients.DomainModels;

namespace Seller.Web.Shared.Repositories.Clients
{
    public interface IClientsRepository
    {
        Task<Client> GetClientAsync(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<Guid> SaveAsync(string token, string language, Guid? id, string name, string email, string communicationLanguage, string country, string phoneNumber, Guid organisationId, IEnumerable<Guid> clientGroupIds, IEnumerable<Guid> clientManagerIds);
        Task<PagedResults<IEnumerable<Client>>> GetClientsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<IEnumerable<Client>> GetAllClientsAsync(string token, string language);
        Task<IEnumerable<Client>> GetClientsAsync(string token, string language, IEnumerable<Guid> clientIds);
    }
}
