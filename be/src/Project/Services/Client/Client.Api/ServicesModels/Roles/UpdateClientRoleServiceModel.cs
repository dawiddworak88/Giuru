using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Roles
{
    public class UpdateClientRoleServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}
