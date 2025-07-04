using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Services.Claims;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.FieldValues;
using Seller.Web.Areas.Clients.Repositories.Groups;
using Seller.Web.Areas.Shared.Repositories.UserApprovals;
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Shared.Repositories.Identity;
using Seller.Web.Shared.Repositories.Organisations;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientsApiController : BaseApiController
    {
        private readonly IOrganisationsRepository _organisationsRepository;
        private readonly IClientsRepository _clientsRepository;
        private readonly IIdentityRepository _identityRepository;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IClientGroupsRepository _clientGroupsRepository;
        private readonly IClientFieldValuesRepository _clientFieldValuesRepository;
        private readonly IUserApprovalsRepository _userApprovalsRepository;
        private readonly IClaimsCacheInvalidatorService _cacheInvalidatorService;

        public ClientsApiController(
            IOrganisationsRepository organisationsRepository,
            IClientsRepository clientsRepository,
            IStringLocalizer<ClientResources> clientLocalizer,
            IIdentityRepository identityRepository,
            IClientGroupsRepository clientGroupsRepository,
            IClientFieldValuesRepository clientFieldValuesRepository,
            IUserApprovalsRepository userApprovalsRepository,
            IClaimsCacheInvalidatorService cacheInvalidatorService)
        {
            _organisationsRepository = organisationsRepository;
            _clientsRepository = clientsRepository;
            _clientLocalizer = clientLocalizer;
            _identityRepository = identityRepository;
            _clientGroupsRepository = clientGroupsRepository;
            _clientFieldValuesRepository = clientFieldValuesRepository;
            _userApprovalsRepository = userApprovalsRepository;
            _cacheInvalidatorService = cacheInvalidatorService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var clients = await _clientsRepository.GetClientsAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(Client.CreatedDate)} desc");

            return StatusCode((int)HttpStatusCode.OK, clients);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveClientRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            Guid? organisationId;

            var organisation = await _organisationsRepository.GetAsync(token, language, model.Email);

            if (organisation is not null)
            {
                organisationId = organisation.Id;
            }
            else
            {
                organisationId = await _organisationsRepository.SaveAsync(token, language, model.Name, model.Email, model.CommunicationLanguage);
            }

            var clientId = await _clientsRepository.SaveAsync(token, language, model.Id, model.Name, model.Email, model.CommunicationLanguage, model.CountryId, model.PreferedCurrencyId, model.PhoneNumber, model.IsDisabled, organisationId.Value, model.ClientGroupIds, model.ClientManagerIds, model.DefaultDeliveryAddressId, model.DefaultBillingAddressId);

            if (model.FieldsValues is not null && model.FieldsValues.Any())
            {
                await _clientFieldValuesRepository.SaveAsync(token, language, clientId,
                  model.FieldsValues.Select(x => new ApiClientFieldValue
                  {
                      FieldDefinitionId = x.FieldDefinitionId,
                      FieldValue = x.FieldValue
                  }));
            }

            if (model.HasAccount)
            {
                await _identityRepository.UpdateAsync(token, language, clientId, model.Email, model.Name, model.CommunicationLanguage, model.IsDisabled);

                if (model.ClientGroupIds.OrEmptyIfNull().Any())
                {
                    var clientGroups = await _clientGroupsRepository.GetClientGroupsAsync(token, language, model.ClientGroupIds);

                    if (clientGroups is not null)
                    {
                        await _identityRepository.AssignRolesAsync(token, language, model.Email, clientGroups.Select(x => x.Name));
                    }
                }

                var user = await _identityRepository.GetAsync(token, language, model.Email);

                await _userApprovalsRepository.SaveAsync(token, language, model.ClientApprovalIds, Guid.Parse(user.Id));
            }

            await _cacheInvalidatorService.InvalidateAsync(model.Email);

            return StatusCode((int)HttpStatusCode.OK, new { Id = clientId, Message = _clientLocalizer.GetString("ClientSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _clientsRepository.DeleteAsync(token, language, id);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _clientLocalizer.GetString("ClientDeletedSuccessfully").Value });
        }
    }
}
