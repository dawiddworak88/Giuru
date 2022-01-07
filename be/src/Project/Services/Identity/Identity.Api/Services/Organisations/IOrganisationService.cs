using Identity.Api.ServicesModels.Organisations;
using System.Threading.Tasks;

namespace Identity.Api.Services.Organisations
{
    public interface IOrganisationService
    {
        Task<OrganisationServiceModel> GetAsync(GetSellerModel serviceModel);
        Task<OrganisationServiceModel> GetAsync(GetOrganisationModel serviceModel);
        Task<OrganisationServiceModel> CreateAsync(CreateOrganisationServiceModel serviceModel);
    }
}
