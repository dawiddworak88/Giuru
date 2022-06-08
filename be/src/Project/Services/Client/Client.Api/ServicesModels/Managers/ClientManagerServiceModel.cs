using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Managers
{
    public class ClientManagerServiceModel : BaseServiceModel 
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
