using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.Areas.Accounts.Models;
using Identity.Api.Areas.Accounts.Validators;
using Identity.Api.Areas.Accounts.ViewModels;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Identity.Api.Areas.Accounts.Services.UserServices;
using IdentityServer4.Services;
using Identity.Api.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System;

namespace Identity.Api.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    [AllowAnonymous]
    public class SignInController : BaseController
    {
        private readonly IOptions<AppSettings> _options;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IUserService _userService;
        private readonly IComponentModelBuilder<SignInComponentModel, SignInViewModel> _signInModelBuilder;
        private readonly ILogger<SignInController> _logger;

        public SignInController(
            IOptions<AppSettings> options,
            IIdentityServerInteractionService interactionService,
            IUserService userService,
            IComponentModelBuilder<SignInComponentModel, SignInViewModel> signInModelBuilder,
            ILogger<SignInController> logger)
        {
            _interactionService = interactionService;
            _userService = userService;
            _signInModelBuilder = signInModelBuilder;
            _options = options;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            var signInComponentModel = new SignInComponentModel
            {
                ReturnUrl = returnUrl,
                DevelopersEmail = _options.Value.DevelopersEmail
            };

            var viewModel = _signInModelBuilder.BuildModel(signInComponentModel);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SignInModel model)
        {
            var validator = new SignInModelValidator();
            var validationResult = await validator.ValidateAsync(model);

            if (validationResult.IsValid)
            {
                try
                {
                    var context = await _interactionService.GetAuthorizationContextAsync(model.ReturnUrl);

                    if (context is not null)
                    {
                        await _userService.SignInAsync(model.Email, model.Password, model.ReturnUrl, context.Client.ClientId);

                        return Redirect(model.ReturnUrl);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Unsuccessful login for {0} user. Error message: {1}", model.Email, ex.Message);

                    var errorViewModel = _signInModelBuilder.BuildModel(new SignInComponentModel { ErrorMessage = ex.Message, ReturnUrl = model.ReturnUrl, DevelopersEmail = _options.Value.DevelopersEmail });

                    return View(errorViewModel);
                }
            }

            var viewModel = _signInModelBuilder.BuildModel(new SignInComponentModel { ReturnUrl = model.ReturnUrl, DevelopersEmail = _options.Value.DevelopersEmail });

            return View(viewModel);
        }
    }
}
