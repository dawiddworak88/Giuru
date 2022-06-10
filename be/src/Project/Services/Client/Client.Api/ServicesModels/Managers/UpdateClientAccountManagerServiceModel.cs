using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Managers
{
    public class UpdateClientAccountManagerServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
