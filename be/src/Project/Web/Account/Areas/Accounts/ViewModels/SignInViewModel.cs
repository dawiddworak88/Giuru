using Account.ViewModels.SignInForm;
using Feature.PageContent.Components.Footers.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;

namespace Account.Areas.Accounts.ViewModels
{
    public class SignInViewModel
    {
        public HeaderViewModel Header { get; set; }
        public SignInFormViewModel SignInForm { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
