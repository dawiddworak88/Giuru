using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.FieldOptions;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientFieldOptionsApiController : BaseApiController
    {
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IClientFieldOptionsRepository _clientFieldOptionsRepository;

        public ClientFieldOptionsApiController(
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientFieldOptionsRepository clientFieldOptionsRepository)
        {
            _clientFieldOptionsRepository = clientFieldOptionsRepository;
            _clientLocalizer = clientLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] ClientFieldOptionRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var fieldOptionId = await _clientFieldOptionsRepository.SaveAsync(token, language, model.Id, model.Name, model.Value, model.FieldDefinitionId);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = fieldOptionId, Message = _clientLocalizer.GetString("FieldOptionSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _clientFieldOptionsRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = _clientLocalizer.GetString("FieldOptionDeletedSuccessfully").Value });
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? fieldDefinitionId, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var fieldOptions = await _clientFieldOptionsRepository.GetAsync(
                token, language, fieldDefinitionId, searchTerm, pageIndex, itemsPerPage, $"{nameof(ClientFieldOption.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, fieldOptions);
        }
    }
}
