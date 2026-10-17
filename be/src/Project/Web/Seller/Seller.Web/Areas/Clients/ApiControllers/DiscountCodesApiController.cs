using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.DiscountCodes;
using Seller.Web.Shared.Configurations;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    // Discount Codes exist only while Grula is configured, so every action answers 404 without it.
    [Area("Clients")]
    public class DiscountCodesApiController : BaseApiController
    {
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IDiscountCodesRepository _discountCodesRepository;
        private readonly IOptions<AppSettings> _options;

        public DiscountCodesApiController(
            IStringLocalizer<ClientResources> clientLocalizer,
            IDiscountCodesRepository discountCodesRepository,
            IOptions<AppSettings> options)
        {
            _clientLocalizer = clientLocalizer;
            _discountCodesRepository = discountCodesRepository;
            _options = options;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            if (!_options.Value.IsGrulaConfigured)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            var discountCodes = await _discountCodesRepository.GetAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(DiscountCode.CreatedDate)} desc");

            return StatusCode((int)HttpStatusCode.OK, discountCodes);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid? id)
        {
            if (!_options.Value.IsGrulaConfigured)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            var discountCode = await _discountCodesRepository.GetAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                id);

            if (discountCode is not null)
            {
                return StatusCode((int)HttpStatusCode.OK, discountCode);
            }

            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] DiscountCodeRequestModel model)
        {
            if (!_options.Value.IsGrulaConfigured)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            var discountCodeId = await _discountCodesRepository.SaveAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                model.Id,
                model.Code,
                model.Description,
                model.IsDisabled,
                model.ClientIds);

            return StatusCode((int)HttpStatusCode.OK, new { Id = discountCodeId, Message = _clientLocalizer.GetString("DiscountCodeSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (!_options.Value.IsGrulaConfigured)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            await _discountCodesRepository.DeleteAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                id);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _clientLocalizer.GetString("DiscountCodeDeletedSuccessfully").Value });
        }
    }
}
