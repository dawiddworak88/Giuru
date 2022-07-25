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
        Task<TeamMember> GetAsync(string token, string language, Guid? id);
    }
}
