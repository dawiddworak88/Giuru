using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.TeamMembers.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.TeamMembers.Repositories
{
    public interface ITeamMembersRepository
    {
        Task<PagedResults<IEnumerable<TeamMember>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<Guid> SaveAsync(string token, string language, Guid? id, string firstName, string lastName, string email, bool isActive, string returnUrl);
        Task<TeamMember> GetAsync(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language, Guid? id);
    }
}
