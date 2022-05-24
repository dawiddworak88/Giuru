using Buyer.Web.Areas.Dashboard.Repositories.Identity;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Dashboard.ApiControllers
{
    [Area("Dashboard")]
    public class IdentityApiController : BaseApiController
    {
        private readonly IIdentityRepository identityRepository;

        public IdentityApiController(
            IIdentityRepository identityRepository)
        {
            this.identityRepository = identityRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Secret()
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var secret = await this.identityRepository.CreateAppSecretAsync(token, language);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = secret, Message = "SUKCES" });
        }
    }
}
