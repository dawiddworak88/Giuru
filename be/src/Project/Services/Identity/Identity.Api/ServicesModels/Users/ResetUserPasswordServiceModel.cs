using Foundation.Extensions.Models;
using Microsoft.AspNetCore.Http;

namespace Identity.Api.ServicesModels.Users
{
    public class ResetUserPasswordServiceModel : BaseServiceModel
    {
        public string Email { get; set; }
        public string ReturnUrl { get; set; }
        public string Scheme { get; set; }
        public HostString Host { get; set; }
    }
}
