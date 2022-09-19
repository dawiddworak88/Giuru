using System;

namespace Client.Api.ServicesModels.Roles
{
    public class ClientRoleServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
