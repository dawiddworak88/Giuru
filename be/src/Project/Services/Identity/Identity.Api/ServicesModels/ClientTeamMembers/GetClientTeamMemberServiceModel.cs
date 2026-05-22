using Foundation.Extensions.Models;
using System;

namespace Identity.Api.ServicesModels.ClientTeamMembers
{
    public class GetClientTeamMemberServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public bool IsSeller { get; set; }
    }
}
