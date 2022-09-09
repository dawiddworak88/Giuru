using Seller.Web.Areas.TeamMembers.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.TeamMembers.ViewModel
{
    public class TeamMembersPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<TeamMember> Catalog { get; set; }
    }
}
