using Feature.Account;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.Areas.Accounts.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.ModelBuilders
{
    public class ResetPasswordFormModelBuilder : IAsyncComponentModelBuilder<ResetPasswordComponentModel, ResetPasswordFormViewModel>
    {
        private readonly IStringLocalizer<AccountResources> accountLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ResetPasswordFormModelBuilder(
            IStringLocalizer<AccountResources> accountLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            this.accountLocalizer = accountLocalizer;
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<ResetPasswordFormViewModel> BuildModelAsync(ResetPasswordComponentModel componentModel)
        {
            var viewModel = new ResetPasswordFormViewModel
            {
                EmailFormatErrorMessage = this.globalLocalizer.GetString("EmailFormatErrorMessage"),
                EmailRequiredErrorMessage = this.globalLocalizer.GetString("EmailRequiredErrorMessage"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                ResetPasswordText = this.accountLocalizer.GetString("ResetPassword"),
                EmailLabel = this.globalLocalizer.GetString("EmailLabel"),
                SubmitUrl = this.linkGenerator.GetPathByAction("ResetPassword", "IdentityApi", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name })
            };

            return viewModel;
        }
    }
}