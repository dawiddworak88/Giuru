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
        private readonly IUserService _userService;
        private readonly IUsersService _usersService;
        private readonly IOptions<AppSettings> _options;
        private readonly IStringLocalizer<AccountResources> _accountLocalizer;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public IdentityApiController(
            IUserService userService,
            IOptions<AppSettings> options,
            IStringLocalizer<AccountResources> accountLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IUsersService usersService)
        {
            _userService = userService;
            _usersService = usersService;
            _options = options;
            _accountLocalizer = accountLocalizer;
            _globalLocalizer = globalLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? id)
        {
            var language = CultureInfo.CurrentUICulture.Name;

            var user = await _usersService.GetById(new GetUserServiceModel
            { 
                Language = language,
                Id = id
            });

            return StatusCode((int)HttpStatusCode.OK, user);
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
                    ReturnUrl = _options.Value.BuyerUrl,
                    Scheme = HttpContext.Request.Scheme,
                    Host = HttpContext.Request.Host
                };

                await _usersService.ResetPasswordAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK, new { Message = _accountLocalizer.GetString("SuccessfullyResetPassword").Value });
            }

            return StatusCode((int)HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SetUserPasswordRequestModel model)
        {
            var serviceModel = new SetUserPasswordServiceModel
            {
                ExpirationId = model.Id.Value,
                Password = model.Password,
                Language = CultureInfo.CurrentUICulture.Name
            };

            var validator = new SetPasswordModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                try
                {
                    var user = await _usersService.SetPasswordAsync(serviceModel);

                    if (user is not null)
                    {
                        await _userService.SignInAsync(user.Email, model.Password, model.ReturnUrl, null);

                        return StatusCode((int)HttpStatusCode.OK, new { Url = _options.Value.BuyerUrl });
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { Message = ex.Message, UrlLabel = _globalLocalizer.GetString("TrySignIn").Value, Url = _options.Value.BuyerUrl });
                }
            }   

            return StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}