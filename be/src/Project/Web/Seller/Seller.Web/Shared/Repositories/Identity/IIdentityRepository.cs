using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.DomainModels.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Repositories.Identity
{
    public interface IIdentityRepository
    {
        Task<Guid> SaveAsync(string token, string language, string name, string email, string communicationLanguage, string returnUrl);
        Task<Guid> UpdateAsync(string token, string language, Guid? id, string email, string name, string communicationLanguage);
        Task<User> GetAsync(string token, string language, string email);
        Task<PagedResults<IEnumerable<ClientApplication>>> GetRegisterApplicationsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
    }
}
