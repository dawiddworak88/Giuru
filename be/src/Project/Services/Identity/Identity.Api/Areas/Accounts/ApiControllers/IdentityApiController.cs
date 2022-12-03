using Feature.Account;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Exceptions;
using Foundation.Localization;
using Identity.Api.Areas.Accounts.ApiRequestModels;
using Identity.Api.Areas.Accounts.Services.UserServices;
using Identity.Api.Areas.Accounts.Validators;
using Identity.Api.Configurations;
using Identity.Api.Services.Users;
using Identity.Api.ServicesModels.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.ApiControllers
{
    [Area("Accounts")]
    public class IdentityApiController : BaseApiController
    {
        private readonly IUserService userService;
        private readonly IUsersService usersService;
        private readonly IOptions<AppSettings> options;
        private readonly IStringLocalizer<AccountResources> accountLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;

        public IdentityApiController(
            IUserService userService,
            IOptions<AppSettings> options,
            IStringLocalizer<AccountResources> accountLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator,
            IUsersService usersService)
        {
            this.userService = userService;
            this.usersService = usersService;
            this.options = options;
            this.accountLocalizer = accountLocalizer;
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? id)
        {
            var language = CultureInfo.CurrentUICulture.Name;

            var user = await this.usersService.GetById(new GetUserServiceModel
            { 
                Language = language,
                Id = id
            });

            return this.StatusCode((int)HttpStatusCode.OK, user);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetUserPasswordRequestModel model)
        {
            var validator = new ResetPasswordModelValidator();
            var result = await validator.ValidateAsync(model);
            if (result.IsValid)
            {
                var serviceModel = new ResetUserPasswordServiceModel {
                    Email = model.Email,
                    ReturnUrl = this.options.Value.BuyerUrl,
                    Scheme = this.HttpContext.Request.Scheme,
                    Host = this.HttpContext.Request.Host
                };

                await this.usersService.ResetPasswordAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.accountLocalizer.GetString("SuccessfullyResetPassword").Value });
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
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

                var user = await this.usersService.SetPasswordAsync(serviceModel);

                if (user is not null)
                {
                    if (await this.userService.SignInAsync(user.Email, model.Password, null, null))
                    {
                        return this.StatusCode((int)HttpStatusCode.Redirect, new { Url = model.ReturnUrl });
                    }
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.BadRequest, new { EmailIsConfirmedLabel = this.accountLocalizer.GetString("EmailIsConfirmed").Value, SignInLabel = this.globalLocalizer.GetString("TrySignIn").Value, SignInUrl = this.linkGenerator.GetPathByAction("Index", "SignIn", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name })});
                }
            }   

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}