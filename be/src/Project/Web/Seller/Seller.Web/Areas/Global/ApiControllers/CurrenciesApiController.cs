using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Services.Cache;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Global.ApiRequestModels;
using Seller.Web.Areas.Global.Definitions;
using Seller.Web.Areas.Global.DomainModels;
using Seller.Web.Areas.Global.Repositories;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Global.ApiControllers
{
    [Area("Global")]
    public class CurrenciesApiController : ControllerBase
    {
        private readonly ICurrenciesRepository _currenciesRepository;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly ICacheService _cacheService;

        public CurrenciesApiController(
            ICurrenciesRepository currenciesRepository, 
            IStringLocalizer<GlobalResources> globalLocalizer,
            ICacheService cacheService)
        {
            _currenciesRepository = currenciesRepository;
            _globalLocalizer = globalLocalizer;
            _cacheService = cacheService;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] CurrencyRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _currenciesRepository.SaveAsync(token, language, model.Id, model.CurrencyCode, model.Symbol, model.Name);

            await _cacheService.InvalidateAsync(GlobalConstants.CurrenciesCacheKey);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _globalLocalizer.GetString("CurrencySavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _currenciesRepository.DeleteAsync(token, language, id);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _globalLocalizer.GetString("CurrencyDeletedSuccessfully").Value });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var currencies = await _currenciesRepository.GetAsync(token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(Currency.CreatedDate)} desc");

            return StatusCode((int)HttpStatusCode.OK, currencies);
        }
    }
}
