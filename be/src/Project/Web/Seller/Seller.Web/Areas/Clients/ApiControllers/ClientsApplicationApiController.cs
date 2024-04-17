using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.Applications;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientsApplicationApiController : BaseApiController
    {
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IClientApplicationsRepository _clientApplicationsRepository;

        public ClientsApplicationApiController(
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientApplicationsRepository clientApplicationsRepository)
        {
            _clientLocalizer = clientLocalizer;
            _clientApplicationsRepository = clientApplicationsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var groups = await _clientApplicationsRepository.GetAsync(token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(ClientApplication.CreatedDate)} desc");

            return StatusCode((int)HttpStatusCode.OK, groups);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _clientApplicationsRepository.DeleteAsync(token, language, id);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _clientLocalizer.GetString("ApplicationDeletedSuccessfully").Value });
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] ClientApplicationRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var clientApplication = await _clientApplicationsRepository.SaveAsync(
                token, language, model.Id, model.CompanyName, model.FirstName, model.LastName, model.ContactJobTitle,
                model.Email, model.PhoneNumber, model.CommunicationLanguage, model.IsDeliveryAddressEqualBillingAddress,
                model.BillingAddress, model.DeliveryAddress);

            return StatusCode((int)HttpStatusCode.OK, new { Id = clientApplication, Message = _clientLocalizer.GetString("ClientApplicationSavedSuccessfully").Value });
        }
    }
}
