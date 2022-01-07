using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Options;
using Seller.Web.Shared.ApiRequestModels;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.DomainModels.Organisations;
using System;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Repositories.Organisations
{
    public class OrganisationsRepository : IOrganisationsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public OrganisationsRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<Organisation> GetAsync(string token, string language, string email)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Identity.OrganisationsApiEndpoint}/{email}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, Organisation>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return response.Data;
            }

            return null;
        }

        public async Task<Guid> SaveAsync(string token, string language, string name, string email, string communicationLanguage)
        {
            var apiRequest = new ApiRequest<SaveOrganisationRequestModel>
            {
                Language = language,
                Data = new SaveOrganisationRequestModel
                {
                    Email = email,
                    Name = name,
                    CommunicationLanguage = communicationLanguage
                },
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.IdentityUrl}{ApiConstants.Identity.OrganisationsApiEndpoint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<SaveOrganisationRequestModel>, SaveOrganisationRequestModel, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode || response?.Data?.Id == null)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode)
            {
                return response.Data.Id.Value;
            }

            return default;
        }
    }
}
