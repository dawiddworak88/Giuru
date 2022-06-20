using Foundation.Extensions.Models;

namespace Client.Api.ServicesModels.Managers
{
    public class CreateClientAccountManagerServiceModel : BaseServiceModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
