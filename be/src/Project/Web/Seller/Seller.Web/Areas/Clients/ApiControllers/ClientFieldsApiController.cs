using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.Fields;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientFieldsApiController : BaseApiController
    {
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IClientFieldsRepository _clientFieldsRepository;

        public ClientFieldsApiController(
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientFieldsRepository clientFieldsRepository)
        {
            _clientLocalizer = clientLocalizer;
            _clientFieldsRepository = clientFieldsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] ClientFieldRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var fieldId = await _clientFieldsRepository.SaveAsync(token, language, model.Id, model.Name, model.Type);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = fieldId, Message = _clientLocalizer.GetString("FieldSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _clientFieldsRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = _clientLocalizer.GetString("FieldDeletedSuccessfully").Value });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var groups = await _clientFieldsRepository.GetAsync(
                token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(ClientField.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, groups);
        }
    }
}
