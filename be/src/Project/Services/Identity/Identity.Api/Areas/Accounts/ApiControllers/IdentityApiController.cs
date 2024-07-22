using Feature.Account;
using Foundation.ApiExtensions.Controllers;
using Foundation.Localization;
using Identity.Api.Areas.Accounts.ApiRequestModels;
using Identity.Api.Areas.Accounts.Models;
using Identity.Api.Areas.Accounts.Repositories.ClientNotificationTypes;
using Identity.Api.Areas.Accounts.Repositories.Clients;
using Identity.Api.Areas.Accounts.Services.UserServices;
using Identity.Api.Areas.Accounts.Validators;
using Identity.Api.Configurations;
using Identity.Api.Services.Tokens;
using Identity.Api.Services.Users;
using Identity.Api.ServicesModels.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Linq;
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
        private readonly LinkGenerator _linkGenerator;
        private readonly IClientRepository _clientRepository;
        private readonly ITokenService _tokenService;
        private readonly IClientNotificationTypesRepository _clientNotificationTypeRepository;
        private readonly ILogger<IdentityApiController> _logger;

        public IdentityApiController(
            IUserService userService,
            IOptions<AppSettings> options,
            IStringLocalizer<AccountResources> accountLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator,
            IUsersService usersService,
            IClientRepository clientRepository,
            ITokenService tokenService,
            IClientNotificationTypesRepository clientNotificationTypeRepository,
            ILogger<IdentityApiController> logger)
        {
            _userService = userService;
            _usersService = usersService;
            _options = options;
            _accountLocalizer = accountLocalizer;
            _globalLocalizer = globalLocalizer;
            _linkGenerator = linkGenerator;
            _clientRepository = clientRepository;
            _tokenService = tokenService;
            _clientNotificationTypeRepository = clientNotificationTypeRepository;
            _logger = logger;
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
            var language = CultureInfo.CurrentUICulture.Name;

            var serviceModel = new SetUserPasswordServiceModel
            {
                ExpirationId = model.Id.Value,
                Password = model.Password,
                Language = language
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
                        await _userService.SignInAsync(user.Email, model.Password, null, null);

                        if (model.ClientApprovals.Any())
                        {
                            var token = await _tokenService.GetTokenAsync(_options.Value.ApiEmail, _options.Value.ApiOrganisationId, _options.Value.ApiAppSecret);

                            var client = await _clientRepository.GetByOrganisationAsync(token, language, user.OrganisationId);
                            
                            if (client is not null && client.Id.HasValue)
                            {
                                var clientApprovals = new ClientNotificationTypeApprovals
                                {
                                    ClientId = client.Id.Value,
                                    ClientApprovals = model.ClientApprovals
                                };

                                await _clientNotificationTypeRepository.SaveAsync(token, language, clientApprovals);
                            }
                        }

                        return StatusCode((int)HttpStatusCode.Redirect, new { Url = string.IsNullOrWhiteSpace(model.ReturnUrl) ? _options.Value.BuyerUrl : model.ReturnUrl });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while saving the new password.");

                    return StatusCode((int)HttpStatusCode.BadRequest, new { Message = _accountLocalizer.GetString("EmailIsConfirmed").Value, SignInLabel = _globalLocalizer.GetString("TrySignIn").Value, SignInUrl = _linkGenerator.GetPathByAction("Index", "SignIn", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name }) });
                }
            }

            return StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}