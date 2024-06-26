using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.NotificationTypes;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientNotificationTypeApiController : ControllerBase
    {
        private readonly IClientNotificationTypesRepository _clientNotificationTypesRepository;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;

        public ClientNotificationTypeApiController(
            IClientNotificationTypesRepository clientNotificationTypesRepository,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            _clientNotificationTypesRepository = clientNotificationTypesRepository;
            _clientLocalizer = clientLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var notificationTypes = await _clientNotificationTypesRepository.GetAsync(token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(ClientNotificationType.CreatedDate)} desc");

            return StatusCode((int)HttpStatusCode.OK, notificationTypes);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _clientNotificationTypesRepository.DeleteAsync(token, language, id);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _clientLocalizer.GetString("NotificationTypeDeletedSuccessfully").Value });
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] ClientNotificationTypeRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _clientNotificationTypesRepository.SaveAsync(token, language, model.Id, model.Name);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _clientLocalizer.GetString("NotificationTypeSavedSuccessfully").Value });
        }
    }
}
