using Foundation.Extensions.Models;
using Microsoft.AspNetCore.Http;

namespace Identity.Api.ServicesModels.ClientTeamMembers
{
    public class CreateClientTeamMemberServiceModel : BaseServiceModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsDisabled { get; set; }
        public string Scheme { get; set; }
        public HostString Host { get; set; }
        public string ReturnUrl { get; set; }
        public bool IsSeller { get; set; }
    }
}
