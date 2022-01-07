using Feature.Account;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Identity.Api.Areas.Accounts.ApiRequestModels;
using Identity.Api.Areas.Accounts.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.ApiControllers
{
    [Area("Accounts")]
    public class IdentityApiController : BaseApiController
    {
        private readonly IIdentityRepository identityRepository;
        private readonly IStringLocalizer accountLocalizer;

        public IdentityApiController(
            IIdentityRepository identityRepository,
            IStringLocalizer<AccountResources> accountLocalizer)
        {
            this.identityRepository = identityRepository;
            this.accountLocalizer = accountLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var user = await this.identityRepository.GetUserAsync(Guid.Parse(id), token, language);

            return this.StatusCode((int)HttpStatusCode.OK, user);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SetUserPasswordRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var user = this.identityRepository.SetPassword(model.ExpirationId, model.Password, token, language);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = user.Id, Message = this.accountLocalizer.GetString("PasswordUpdated").Value });
        }
    }
}
