using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.FieldOptions
{
    public interface IClientFieldOptionsRepository
    {
        Task<Guid> SaveAsync(string token, string language, Guid? id, string name, Guid? fieldDefinitionId);
        Task<ClientFieldOption> GetAsync(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<PagedResults<IEnumerable<ClientFieldOption>>> GetAsync(string token, string language, Guid? fieldDefinitionId, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
    }
}
