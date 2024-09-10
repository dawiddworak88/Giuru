using Foundation.Extensions.Models;

namespace Client.Api.ServicesModels.Clients
{
    public class GetClientByEmailServiceModel : BaseServiceModel
    {
        public string Email { get; set; }
    }
}
