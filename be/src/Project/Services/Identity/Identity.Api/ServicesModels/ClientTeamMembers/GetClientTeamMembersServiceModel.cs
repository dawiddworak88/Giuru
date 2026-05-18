using Foundation.Extensions.Models;

namespace Identity.Api.ServicesModels.ClientTeamMembers
{
    public class GetClientTeamMembersServiceModel : PagedBaseServiceModel
    {
        public bool IsSeller { get; set; }
    }
}
