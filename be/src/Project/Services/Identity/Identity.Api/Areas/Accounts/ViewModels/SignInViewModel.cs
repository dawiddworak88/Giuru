using Identity.Api.ViewModels.SignInForm;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;

namespace Identity.Api.Areas.Accounts.ViewModels
{
    public class SignInViewModel
    {
        public HeaderViewModel Header { get; set; }
        public SignInFormViewModel SignInForm { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
