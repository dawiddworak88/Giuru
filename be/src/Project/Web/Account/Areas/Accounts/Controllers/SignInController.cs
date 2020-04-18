using Account.Areas.Accounts.ComponentModels;
using Account.Areas.Accounts.Models;
using Account.Areas.Accounts.Validators;
using Account.Areas.Accounts.ViewModels;
using Feature.Account.Services.UserServices;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Account.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    [AllowAnonymous]
    public class SignInController : BaseController
    {
        private readonly IUserService userService;
        private readonly IComponentModelBuilder<SignInComponentModel, SignInViewModel> signInModelBuilder;

        public SignInController(
            IComponentModelBuilder<SignInComponentModel, SignInViewModel> signInModelBuilder,
            IUserService userService
            )
        {
            this.signInModelBuilder = signInModelBuilder;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            var viewModel = this.signInModelBuilder.BuildModel(new SignInComponentModel { ReturnUrl = returnUrl });

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SignInModel model)
        {
            var validator = new SignInModelValidator();

            var result = await validator.ValidateAsync(model);

            if (result.IsValid)
            {
                await this.userService.SignInAsync(this.HttpContext, model.Email, model.Password, model.ReturnUrl);

                return this.Redirect(model.ReturnUrl);
            }

            var viewModel = this.signInModelBuilder.BuildModel(new SignInComponentModel { ReturnUrl = model.ReturnUrl });

            return this.View(viewModel);
        }
    }
}
