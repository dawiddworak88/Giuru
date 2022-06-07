using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Roles
{
    public class DeleteClientRoleServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
