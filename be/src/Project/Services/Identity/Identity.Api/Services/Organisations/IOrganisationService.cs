using Identity.Api.ServicesModels.Organisations;
using System;
using System.Threading.Tasks;

namespace Identity.Api.Services.Organisations
{
    public interface IOrganisationService
    {
        Task<bool> IsSellerAsync(Guid id);
        Task<OrganisationServiceModel> GetAsync(GetSellerModel serviceModel);
        Task<OrganisationServiceModel> GetAsync(GetOrganisationModel serviceModel);
        Task<OrganisationServiceModel> CreateAsync(CreateOrganisationServiceModel serviceModel);
        Task<OrganisationServiceModel> UpdateAsync(UpdateOrganisationServiceModel serviceModel);
        Task<bool> IsDisabledAsync(Guid id);
        Task UpdateStatusAsync(UpdateOrganisationStatusServiceModel serviceModel);
    }
}
