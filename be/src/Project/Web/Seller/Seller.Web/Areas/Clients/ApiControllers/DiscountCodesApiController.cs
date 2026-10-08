using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Foundation.Pricing.DiscountCodes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Policy = "SellerOnly")]
    public class DiscountCodesApiController : BaseApiController
    {
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IDiscountCodesRepository _discountCodesRepository;
        private readonly IGrulaDiscountCodeService _grulaDiscountCodeService;

        public DiscountCodesApiController(
            IStringLocalizer<ClientResources> clientLocalizer,
            IDiscountCodesRepository discountCodesRepository,
            IGrulaDiscountCodeService grulaDiscountCodeService)
        {
            _clientLocalizer = clientLocalizer;
            _discountCodesRepository = discountCodesRepository;
            _grulaDiscountCodeService = grulaDiscountCodeService;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] DiscountCodeRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            // The code text cannot change after creation, so an update is never checked against Grula.
            if (model.Id.HasValue)
            {
                var updatedId = await _discountCodesRepository.SaveAsync(token, language, model.Id, null, model.Description, model.IsDisabled);

                return Saved(updatedId);
            }

            var code = model.Code?.Trim();

            if (string.IsNullOrEmpty(code))
            {
                return Error(HttpStatusCode.UnprocessableEntity, "DiscountCodeRequiredErrorMessage");
            }

            if (code.Length > DiscountCodeLimits.MaxLength)
            {
                return Error(HttpStatusCode.UnprocessableEntity, "DiscountCodeMaxLengthErrorMessage");
            }

            var grulaDiscountCode = await _grulaDiscountCodeService.FindAsync(code, HttpContext.RequestAborted);

            switch (grulaDiscountCode.Status)
            {
                case GrulaDiscountCodeLookupStatus.Exists:
                    break;
                case GrulaDiscountCodeLookupStatus.Missing:
                    return Error(HttpStatusCode.UnprocessableEntity, "DiscountCodeNotFoundInGrula");
                case GrulaDiscountCodeLookupStatus.NotConfigured:
                    return NotFound();
                default:
                    // Unavailable, and anything unforeseen: a code that could not be checked is never saved.
                    return Error(HttpStatusCode.ServiceUnavailable, "DiscountCodeGrulaUnavailable");
            }

            // The code takes Grula's spelling, so what is stored is what Grula matches.
            var canonicalCode = grulaDiscountCode.DiscountCode?.Trim();

            if (string.IsNullOrEmpty(canonicalCode) || canonicalCode.Length > DiscountCodeLimits.MaxLength)
            {
                return Error(HttpStatusCode.UnprocessableEntity, "DiscountCodeMaxLengthErrorMessage");
            }

            var id = await _discountCodesRepository.SaveAsync(token, language, null, canonicalCode, model.Description, model.IsDisabled);

            return Saved(id);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _discountCodesRepository.DeleteAsync(token, language, id);

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

        private IActionResult Saved(Guid id)
        {
            return StatusCode((int)HttpStatusCode.OK, new { Id = id, Message = _clientLocalizer.GetString("DiscountCodeSavedSuccessfully").Value });
        }

        private IActionResult Error(HttpStatusCode statusCode, string resourceKey)
        {
            return StatusCode((int)statusCode, new { Message = _clientLocalizer.GetString(resourceKey).Value });
        }
    }
}
