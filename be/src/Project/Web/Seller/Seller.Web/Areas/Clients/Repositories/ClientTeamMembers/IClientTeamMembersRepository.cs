using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.ClientTeamMembers
{
    public interface IClientTeamMembersRepository
    {
        Task<PagedResults<IEnumerable<ClientTeamMember>>> GetAsync(string token, string language, Guid? organisationId, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<Guid> SaveAsync(string token, string language, Guid? organisationId, Guid? id, string firstName, string lastName, string email, bool isDisabled, string returnUrl);
        Task<ClientTeamMember> GetAsync(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language, Guid? id);
    }
}
