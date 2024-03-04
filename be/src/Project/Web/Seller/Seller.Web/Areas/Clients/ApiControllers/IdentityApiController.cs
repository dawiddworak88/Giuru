using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.Repositories.Identity;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class IdentityApiController : BaseApiController
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IStringLocalizer _clientLocalizer;
        private readonly IOptionsMonitor<AppSettings> _options;

        public IdentityApiController(
            IIdentityRepository identityRepository,
            IStringLocalizer<ClientResources> clientLocalizer,
            IOptionsMonitor<AppSettings> options)
        {
            _identityRepository = identityRepository;
            _clientLocalizer = clientLocalizer;
            _options = options;
        }

        [HttpPost]
        public async Task<IActionResult> Account([FromBody] ClientRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var userId = await _identityRepository.SaveAsync(token, language, model.Name, model.Email, model.CommunicationLanguage, _options.CurrentValue.BuyerUrl);

            return StatusCode((int)HttpStatusCode.OK, new { Id = userId, Message = _clientLocalizer.GetString("AccountCreated").Value});
        }
    }
}
