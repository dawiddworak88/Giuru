using Account.Shared.Footers.ViewModels;
using Account.Shared.Headers.ViewModels;
using Feature.Account.ViewModels.SignInForm;

namespace Account.Areas.Accounts.ViewModels
{
    public class SignInViewModel
    {
        public HeaderViewModel Header { get; set; }
        public SignInFormViewModel SignInForm { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
