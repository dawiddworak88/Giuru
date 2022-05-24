using Buyer.Web.Areas.Dashboard.Repositories.Identity;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Dashboard.ApiControllers
{
    [Area("Dashboard")]
    public class IdentityApiController : BaseApiController
    {
        private readonly IIdentityRepository identityRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public IdentityApiController(
            IIdentityRepository identityRepository,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.identityRepository = identityRepository;
            this.globalLocalizer = globalLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Secret()
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var secret = await this.identityRepository.CreateAppSecretAsync(token, language);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = secret, Message = this.globalLocalizer.GetString("SuccessfullyCreatedAppSecret").Value });
        }
    }
}
