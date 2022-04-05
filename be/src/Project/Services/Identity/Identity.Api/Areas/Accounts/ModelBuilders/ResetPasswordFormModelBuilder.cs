using Feature.Account;
using Foundation.Extensions.ModelBuilders;
using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.Areas.Accounts.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.ModelBuilders
{
    public class ResetPasswordFormModelBuilder : IAsyncComponentModelBuilder<ResetPasswordComponentModel, ResetPasswordFormViewModel>
    {
        private readonly IStringLocalizer<AccountResources> accountLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ResetPasswordFormModelBuilder(
            IStringLocalizer<AccountResources> accountLocalizer,
            LinkGenerator linkGenerator)
        {
            this.accountLocalizer = accountLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<ResetPasswordFormViewModel> BuildModelAsync(ResetPasswordComponentModel componentModel)
        {
            var viewModel = new ResetPasswordFormViewModel
            {
               
            };

            return viewModel;
        }
    }
}