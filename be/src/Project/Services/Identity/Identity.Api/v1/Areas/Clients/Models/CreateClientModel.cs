using Foundation.Extensions.Models;

namespace Identity.Api.v1.Areas.Clients.Models
{
    public class CreateClientModel : BaseServiceModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
    }
}