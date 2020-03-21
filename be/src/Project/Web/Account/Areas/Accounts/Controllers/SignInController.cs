using Account.Areas.Accounts.ViewModels;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Account.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    [AllowAnonymous]
    public class SignInController : BaseController
    {
        private readonly IModelBuilder<SignInViewModel> signInModelBuilder;

        public SignInController(IModelBuilder<SignInViewModel> signInModelBuilder)
        {
            this.signInModelBuilder = signInModelBuilder;
        }

        public IActionResult Index()
        {
            var viewModel = this.signInModelBuilder.BuildModel();

            return this.View(viewModel);
        }
    }
}
