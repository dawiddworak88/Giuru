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
        private readonly IIdentityRepository identityRepository;
        private readonly IStringLocalizer clientLocalizer;
        private readonly IOptionsMonitor<AppSettings> options;

        public IdentityApiController(
            IIdentityRepository identityRepository,
            IStringLocalizer<ClientResources> clientLocalizer,
            IOptionsMonitor<AppSettings> options)
        {
            this.identityRepository = identityRepository;
            this.clientLocalizer = clientLocalizer;
            this.options = options;
        }

        [HttpPost]
        public async Task<IActionResult> Account([FromBody] ClientRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var userId = await this.identityRepository.SaveAsync(token, language, model.Name, model.Email, model.CommunicationLanguage, this.options.CurrentValue.BuyerUrl);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = userId, Message = this.clientLocalizer.GetString("AccountCreated").Value});
        }
    }
}
