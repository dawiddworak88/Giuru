using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.DiscountCodes;
using Seller.Web.Shared.Filters;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    [RequireGrula]
    public class DiscountCodesApiController : BaseApiController
    {
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IDiscountCodesRepository _discountCodesRepository;

        public DiscountCodesApiController(
            IStringLocalizer<ClientResources> clientLocalizer,
            IDiscountCodesRepository discountCodesRepository)
        {
            _clientLocalizer = clientLocalizer;
            _discountCodesRepository = discountCodesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] DiscountCodeRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            // The code is not checked against Grula: Giuru never creates a code there or asks whether it exists.
            var saved = await _discountCodesRepository.SaveAsync(
                token, language, model.Id, model.Version, model.Code, model.Description, model.IsDisabled, model.ClientIds);

            return StatusCode((int)HttpStatusCode.OK, new { saved.Id, saved.Version, Message = _clientLocalizer.GetString("DiscountCodeSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id, [FromQuery(Name = "version")] string version)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _discountCodesRepository.DeleteAsync(token, language, id, version);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _clientLocalizer.GetString("DiscountCodeDeletedSuccessfully").Value });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var discountCodes = await _discountCodesRepository.GetAsync(
                token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(DiscountCode.CreatedDate)} desc");

            return StatusCode((int)HttpStatusCode.OK, discountCodes);
        }
    }
}
