using Foundation.Extensions.Models;

namespace Identity.Api.ServicesModels.Organisations
{
    public class CreateOrganisationServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationsLanguage { get; set; }
    }
}
