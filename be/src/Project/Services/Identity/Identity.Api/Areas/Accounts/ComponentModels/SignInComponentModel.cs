using Microsoft.AspNetCore.Http;

namespace Identity.Api.Areas.Accounts.ComponentModels
{
    public class SignInComponentModel
    {
        public string ReturnUrl { get; set; }
        public string DevelopersEmail { get; set; }
        public string Scheme { get; set; }
        public HostString Host { get; set; }
    }
}
