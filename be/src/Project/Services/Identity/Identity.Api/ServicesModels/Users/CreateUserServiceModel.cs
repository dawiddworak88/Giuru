using Foundation.Extensions.Models;

namespace Identity.Api.ServicesModels.Users
{
    public class CreateUserServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationsLanguage { get; set; }
    }
}
