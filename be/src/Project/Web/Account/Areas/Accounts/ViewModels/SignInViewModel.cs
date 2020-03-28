using Feature.Account.ViewModels.SignInForm;
using Feature.PageContent.Shared.Footers.ViewModels;
using Feature.PageContent.Shared.Headers.ViewModels;

namespace Account.Areas.Accounts.ViewModels
{
    public class SignInViewModel
    {
        public HeaderViewModel Header { get; set; }
        public SignInFormViewModel SignInForm { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
