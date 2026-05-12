using Foundation.Extensions.Models;
using System;

namespace Identity.Api.ServicesModels.ClientTeamMembers
{
    public class UpdateClientTeamMemberServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsSeller { get; set; }
    }
}
