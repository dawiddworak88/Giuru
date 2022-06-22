using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Roles
{
    public class GetClientRoleServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
