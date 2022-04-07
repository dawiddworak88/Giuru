using Feature.Account;
using Foundation.ApiExtensions.Controllers;
using Identity.Api.Areas.Accounts.ApiRequestModels;
using Identity.Api.Areas.Accounts.Services.UserServices;
using Identity.Api.Areas.Accounts.Validators;
using Identity.Api.Services.Users;
using Identity.Api.ServicesModels.Users;
using IdentityServer4.Services;
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
        private readonly IUserService userService;
        private readonly IUsersService usersService;

        public IdentityApiController(
            IUserService userService,
            IUsersService usersService)
        {
            this.userService = userService;
            this.usersService = usersService;
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
                        return this.StatusCode((int)HttpStatusCode.Redirect);
                    }
                }
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
