using Foundation.Extensions.Models;
using System;

namespace Identity.Api.ServicesModels.TeamMembers
{
    public class GetTeamMemberServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
