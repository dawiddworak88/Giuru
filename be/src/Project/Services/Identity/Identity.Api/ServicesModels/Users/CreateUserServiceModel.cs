using Foundation.Extensions.Models;
using Microsoft.AspNetCore.Http;

namespace Identity.Api.ServicesModels.Users
{
    public class CreateUserServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationsLanguage { get; set; }
        public string Scheme { get; set; }
        public HostString Host { get; set; }
        public string ReturnUrl { get; set; }
    }
}
