using Feature.Account;
using Foundation.ApiExtensions.Controllers;
using Identity.Api.Areas.Accounts.ApiRequestModels;
using Identity.Api.Areas.Accounts.Validators;
using Identity.Api.Services.Users;
using Identity.Api.ServicesModels.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.ApiControllers
{
    [Area("Accounts")]
    public class IdentityApiController : BaseApiController
    {
        private readonly IUsersService userService;
        private readonly IStringLocalizer accountLocalizer;

        public IdentityApiController(
            IUsersService userService,
            IStringLocalizer<AccountResources> accountLocalizer)
        {
            this.userService = userService;
            this.accountLocalizer = accountLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? id)
        {
            var language = CultureInfo.CurrentUICulture.Name;

            var user = await this.userService.GetById(new GetUserServiceModel
            { 
                Language = language,
                Id = id
            });

            return this.StatusCode((int)HttpStatusCode.OK, user);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SetUserPasswordRequestModel model)
        {
            var validator = new SetPasswordModelValidator();
            var result = await validator.ValidateAsync(model);
            if (result.IsValid)
            {
                var language = CultureInfo.CurrentUICulture.Name;
                var serviceModel = new SetUserPasswordServiceModel
                {
                    ExpirationId = model.Id.Value,
                    Password = model.Password,
                    Language = language
                };

                var user = await this.userService.SetPasswordAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK, new { Id = user.Id, Message = this.accountLocalizer.GetString("PasswordUpdated").Value });
            }
            return this.StatusCode((int)HttpStatusCode.BadRequest, "asd");
        }
    }
}
