using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Helpers;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Settings.ApiRequestModels;
using Seller.Web.Areas.Settings.Repositories;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Settings.ApiControllers
{
    [Area("Settings")]
    public class SettingsApiController : ControllerBase
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public SettingsApiController(
            ISettingRepository settingRepository,
            IStringLocalizer<GlobalResources> globalLocalizer)
        { 
            _settingRepository = settingRepository;
            _globalLocalizer = globalLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveSettingsRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;
            var sellerId = GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value);

            await _settingRepository.PostAsync(token, language, sellerId, model.Settings);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _globalLocalizer.GetString("SettingsSavedSuccessfully").Value });
        }
    }
}
