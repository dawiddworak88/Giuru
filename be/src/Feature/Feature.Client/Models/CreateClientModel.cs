using Foundation.Extensions.Models;

namespace Feature.Client.Models
{
    public class CreateClientModel : BaseServiceModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ClientPreferredLanguage { get; set; }
    }
}
