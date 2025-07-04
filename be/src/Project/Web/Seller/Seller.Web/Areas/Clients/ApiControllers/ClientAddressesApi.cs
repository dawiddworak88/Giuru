using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Services.Claims;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.DeliveryAddresses;
using Seller.Web.Shared.Repositories.Clients;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientAddressesApi : BaseApiController
    {
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IClientAddressesRepository _deliveryAddressesRepository;
        private readonly IClaimsCacheInvalidatorService _cacheInvalidatorService;
        private readonly IClientsRepository _clientsRepository;

        public ClientAddressesApi(
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientAddressesRepository deliveryAddressesRepository,
            IClaimsCacheInvalidatorService cacheInvalidatorService,
            IClientsRepository clientsRepository)
        {
            _clientLocalizer = clientLocalizer;
            _deliveryAddressesRepository = deliveryAddressesRepository;
            _cacheInvalidatorService = cacheInvalidatorService;
            _clientsRepository = clientsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] DeliveryAddressRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var client = await _clientsRepository.GetClientAsync(token, language, model.ClientId);

            if (client is not null)
            {
                await _cacheInvalidatorService.InvalidateAsync(client.Email);
            }

            var clientAddressId = await _deliveryAddressesRepository.SaveAsync(token, language, model.Id, model.Company, model.FirstName, model.LastName, model.PhoneNumber, model.Street, model.Region, model.PostCode, model.City, model.ClientId, model.CountryId);

            return StatusCode((int)HttpStatusCode.OK, new { Id = clientAddressId, Message = _clientLocalizer.GetString("ClientAddressSavedSuccessfully").Value });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var clientAddresses = await _deliveryAddressesRepository.GetAsync(
                token,
                language,
                null,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(ClientAddress.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, clientAddresses);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _deliveryAddressesRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = _clientLocalizer.GetString("ClientAddressDeletedSuccessfully").Value });
        }
    }
}
